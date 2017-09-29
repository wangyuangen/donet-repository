using System;
using System.Windows.Forms;

namespace Img.ManualUpload.Control
{
    internal class OpaqueCommand
    {
        private MyOpaqueLayer m_OpaqueLayer = null;//°ëÍ¸Ã÷ÃÉ°å²ã

        /// <summary>
        /// ÏÔÊ¾ÕÚÕÖ²ã
        /// </summary>
        /// <param name="control">¿Ø¼þ</param>
        /// <param name="alpha">Í¸Ã÷¶È</param>
        /// <param name="isShowLoadingImage">ÊÇ·ñÏÔÊ¾Í¼±ê</param>
        public void ShowOpaqueLayer(System.Windows.Forms.Control control, int alpha, bool isShowLoadingImage)
         {
            try
            {
                if (this.m_OpaqueLayer == null)
                {
                    this.m_OpaqueLayer = new MyOpaqueLayer(alpha, isShowLoadingImage);
                    control.Controls.Add(this.m_OpaqueLayer);
                    this.m_OpaqueLayer.Dock = DockStyle.Fill;
                    this.m_OpaqueLayer.BringToFront();
                }
                this.m_OpaqueLayer.Enabled = true;
                this.m_OpaqueLayer.Visible = true;
            }
            catch { }
        }

        /// <summary>
        /// Òþ²ØÕÚÕÖ²ã
        /// </summary>
        public void HideOpaqueLayer()
        {
            try
            {
                if (this.m_OpaqueLayer != null)
                {
                    this.m_OpaqueLayer.Visible = false;
                    this.m_OpaqueLayer.Enabled = false;
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

    }
}
