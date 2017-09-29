using Img.Config.config;
using Img.Config.Model;
using Img.DataService;
using Img.DataService.Infrastructure;
using Img.JobLogData;
using Img.ManualUpload.Control;
using Img.Model.Search;
using Img.Nlog;
using Img.Nlog.Imp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Img.ManualUpload
{
    public partial class Form1 : Form
    {
        #region Propety
        private Dictionary<string, IDataService> dicServices;
        private Global global = ConfigManager.Instance.Global;

        private IDataService _dataService;
        private ILogger NLogger;

        private ImageSearch Search { get; set; }
        private DataGridViewCheckBoxHeaderCell CustomHeaderCell;
        private OpaqueCommand cmd;
        #endregion

        #region Ctor
        public Form1()
        {
            InitializeComponent();
            if(string.IsNullOrEmpty(global.Domain)||string.IsNullOrEmpty(global.OfficeName)||
                string.IsNullOrEmpty(global.UserName)||string.IsNullOrEmpty(global.UserPwd))
            {
                MessageBox.Show("请先配置诊所相关信息");
                System.Environment.Exit(0);
            }
            Img.DataService.StartUp.Initialization();
            dicServices = Img.DataService.StartUp.LoadServices();
            //dicConfig = Img.DataService.StartUp.LoadConfig();

            cmd = new OpaqueCommand();

            CheckForIllegalCrossThreadCalls = false;
            CustomHeaderCell = new DataGridViewCheckBoxHeaderCell();
            DataGridViewCheckBoxHeaderCell.OnCheckBoxClicked += CustomHeaderCell_OnCheckBoxClicked;

            dataGridView1.Columns[0].HeaderCell = CustomHeaderCell;
            dataGridView1.Columns[0].HeaderCell.Value = "";

            Init();
        }

        #endregion

        #region Method
        public void CustomHeaderCell_OnCheckBoxClicked(object sender, datagridviewCheckboxHeaderEventArgs e)
        {
            bool flag = e.CheckedState;
            var DataGridViewRows = dataGridView1.Rows;
            foreach (DataGridViewRow item in DataGridViewRows)
            {
                if (item.Cells[5].Value.ToString() == "已上传")
                {
                    item.Cells[6].Value = false;
                    continue;
                }
                item.Cells[6].Value = flag;
            }
        }

        private void Init()
        {

            pagerControl1.OnPageChanged += new EventHandler(pagerControl1_OnPageChanged);

            Search = new ImageSearch()
            {
                StartTime = dateTimePicker1.Value,
                EndTime = dateTimePicker2.Value,
                PageIndex = 1,
                PageSize = 20,
            };

            NLogger = ContainerManager.GetDataService<Logger>("NLog");
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public void LoadData()
        {
            cmd.ShowOpaqueLayer(panel1, 125, true);
            DateTime dtStart = dateTimePicker1.Value;
            DateTime dtEnd = dateTimePicker2.Value;

            //if (Search.BrandType == "Sidexis")
            //{
            //    _dataService = ContainerManager.SidexisDataService;
            //}
            //else if (Search.BrandType == "EasyDent")
            //{
            //    _dataService = ContainerManager.EasyDentDataService;
            //}

            foreach (var service in dicServices)
            {
                if (service.Key.Contains(Search.BrandType))
                {
                    _dataService = service.Value;
                    break;
                }
            }
            if (_dataService != null)
            {
                Search.StartTime = Convert.ToDateTime(dtStart.ToShortDateString());
                Search.EndTime = Convert.ToDateTime(dtEnd.ToShortDateString());
                Search.PageSize = pagerControl1.PageSize;
                Search.PageIndex = pagerControl1.PageIndex;
                var data = _dataService.GenerateImageList(Search);
                SetDgvDataSource(data.Items);
                pagerControl1.DrawControl(data.TotalCount);
                DisableDataGridView();
            }

            cmd.HideOpaqueLayer();
        }

        private void DisableDataGridView()
        {
            var DataGridViewRows = dataGridView1.Rows;
            foreach (DataGridViewRow item in DataGridViewRows)
            {
                if (item.Cells[5].Value.ToString() == "已上传")
                {
                    item.ReadOnly = true;
                }
            }
        }
        #endregion
        #region Event
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value == null || dateTimePicker2.Value == null)
            {
                MessageBox.Show("请选择开始和结束时间");
                return;
            }

            if (Search.BrandType == "--请选择--" || Search.BrandType == "")
            {
                MessageBox.Show("请选择影像品牌");
                return;
            }

            Thread thread = new Thread(new ThreadStart(LoadData));
            thread.IsBackground = true;
            thread.Start();
        }

        delegate void dgvDelegate(IEnumerable<Img.Model.Models.MedicalImage> table);
        private void SetDgvDataSource(IEnumerable<Img.Model.Models.MedicalImage> table)
        {
            if (dataGridView1.InvokeRequired)
            {
                Invoke(new dgvDelegate(SetDgvDataSource), new object[] { table });
            }
            else
            {
                dataGridView1.DataSource = table;
            }
        }

        /// <summary>
        /// 上传影像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpload_Click(object sender, EventArgs e)
        {
            List<Img.Model.Models.MedicalImage> list = new List<Img.Model.Models.MedicalImage>();

            DataGridViewRowCollection collection = dataGridView1.Rows;
            foreach (DataGridViewRow item in collection)
            {
                var row = (item.DataBoundItem as Img.Model.Models.MedicalImage);
                if (row.IsUpload)
                {
                    list.Add(row);
                }
            }

            if (!list.Any())
            {
                MessageBox.Show("请选择要上传的影像");
                return;
            }

            if (_dataService == null)
            {
                MessageBox.Show("未初始化组件");
                return;
            }

            Thread thread = new Thread(() =>
            {

                cmd.ShowOpaqueLayer(panel1, 125, true);

                try
                {
                    _dataService.Upload(list);
                }
                catch (Exception ex)
                {
                    NLogger.Error(ex.Message);
                }
                DateTime dtStart = dateTimePicker1.Value;
                DateTime dtEnd = dateTimePicker2.Value;
                if (dtStart == null || dtEnd == null)
                {
                    return;
                }

                Search.StartTime = Convert.ToDateTime(dtStart.ToShortDateString());
                Search.EndTime = Convert.ToDateTime(dtEnd.ToShortDateString());
                Search.PageSize = pagerControl1.PageSize;
                Search.PageIndex = pagerControl1.PageIndex;
                var data = _dataService.GenerateImageList(Search);
                SetDgvDataSource(data.Items);
                pagerControl1.DrawControl(data.TotalCount);
                DisableDataGridView();
                cmd.HideOpaqueLayer();
            });
            thread.IsBackground = true;
            thread.Start();
        }

        void pagerControl1_OnPageChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0 && e.RowIndex != -1)
            {
                if ((sender as DataGridView).CurrentCell is DataGridViewCheckBoxCell)
                {
                    bool isCheck = (sender as DataGridView).CurrentCell.Selected;
                    var row = this.dataGridView1.Rows[e.RowIndex].DataBoundItem as Img.Model.Models.MedicalImage;
                    if (row.Status == "已上传")
                    {
                        row.IsUpload = false;
                        return;
                    }
                    row.IsUpload = isCheck;
                }
            }
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(global.ManualUploadBrandType))
            {
                return;
            }
            LoadBrandType();
            LoadData();
        }

        private void LoadBrandType()
        {
            var brandtype = global.ManualUploadBrandType;
            brandtype = "--请选择--|" + brandtype;
            cbBrandType.DataSource = brandtype.Split('|');
        }

        private void cbBrandType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Search.BrandType = cbBrandType.SelectedValue.ToString();
        }
    }

    public delegate void DataGridViewCheckBoxHeaderEventHander(object sender, datagridviewCheckboxHeaderEventArgs e);

    public class datagridviewCheckboxHeaderEventArgs : EventArgs
    {
        private bool checkedState = false;

        public bool CheckedState
        {
            get { return checkedState; }
            set { checkedState = value; }
        }
    }

    public class DataGridViewCheckBoxHeaderCell : DataGridViewColumnHeaderCell
    {
        Point checkBoxLocation;
        Size checkBoxSize;
        bool _checked = false;
        Point _cellLocation = new Point();
        System.Windows.Forms.VisualStyles.CheckBoxState _cbState =
            System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;
        public static event DataGridViewCheckBoxHeaderEventHander OnCheckBoxClicked;

        protected override void Paint(
            Graphics graphics,
            Rectangle clipBounds,
            Rectangle cellBounds,
            int rowIndex,
            DataGridViewElementStates dataGridViewElementState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                dataGridViewElementState, value,
                formattedValue, errorText, cellStyle,
                advancedBorderStyle, paintParts);

            Point p = new Point();
            Size s = CheckBoxRenderer.GetGlyphSize(graphics, CheckBoxState.UncheckedNormal);

            p.X = cellBounds.Location.X +
                (cellBounds.Width / 2) - (s.Width / 2) - 1;
            p.Y = cellBounds.Location.Y +
                (cellBounds.Height / 2) - (s.Height / 2);

            _cellLocation = cellBounds.Location;
            checkBoxLocation = p;
            checkBoxSize = s;
            if (_checked)
                _cbState = System.Windows.Forms.VisualStyles.
                    CheckBoxState.CheckedNormal;
            else
                _cbState = System.Windows.Forms.VisualStyles.
                    CheckBoxState.UncheckedNormal;

            CheckBoxRenderer.DrawCheckBox
            (graphics, checkBoxLocation, _cbState);
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            Point p = new Point(e.X + _cellLocation.X, e.Y + _cellLocation.Y);
            if (p.X >= checkBoxLocation.X && p.X <=
                checkBoxLocation.X + checkBoxSize.Width
            && p.Y >= checkBoxLocation.Y && p.Y <=
                checkBoxLocation.Y + checkBoxSize.Height)
            {
                _checked = !_checked;

                datagridviewCheckboxHeaderEventArgs ex = new datagridviewCheckboxHeaderEventArgs();
                ex.CheckedState = _checked;

                object sender = new object();

                if (OnCheckBoxClicked != null)
                {
                    OnCheckBoxClicked(sender, ex);
                    this.DataGridView.InvalidateCell(this);
                }
            }
            base.OnMouseClick(e);
        }


    }
}
