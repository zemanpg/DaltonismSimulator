namespace DaltonismSimulator
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            contextMenuStrip1 = new ContextMenuStrip(components);
            tsiNormal = new ToolStripMenuItem();
            tsiProtanomaly = new ToolStripMenuItem();
            tsiDeuteranomaly = new ToolStripMenuItem();
            tsiTritanomaly = new ToolStripMenuItem();
            skc = new SkiaSharp.Views.Desktop.SKGLControl();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(24, 24);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { tsiNormal, tsiProtanomaly, tsiDeuteranomaly, tsiTritanomaly });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(309, 132);
            // 
            // tsiNormal
            // 
            tsiNormal.Name = "tsiNormal";
            tsiNormal.Size = new Size(308, 32);
            tsiNormal.Text = "Normal View";
            tsiNormal.Click += tsiNormal_Click;
            // 
            // tsiProtanomaly
            // 
            tsiProtanomaly.Name = "tsiProtanomaly";
            tsiProtanomaly.Size = new Size(308, 32);
            tsiProtanomaly.Text = "Red-Weak/Protanomaly";
            tsiProtanomaly.Click += tsiProtanomaly_Click;
            // 
            // tsiDeuteranomaly
            // 
            tsiDeuteranomaly.Name = "tsiDeuteranomaly";
            tsiDeuteranomaly.Size = new Size(308, 32);
            tsiDeuteranomaly.Text = "Green-Weak/Deuteranomaly";
            tsiDeuteranomaly.Click += tsiDeuteranomaly_Click;
            // 
            // tsiTritanomaly
            // 
            tsiTritanomaly.Name = "tsiTritanomaly";
            tsiTritanomaly.Size = new Size(308, 32);
            tsiTritanomaly.Text = "Blue-Weak/Tritanomaly";
            tsiTritanomaly.Click += tsiTritanomaly_Click;
            // 
            // skc
            // 
            skc.BackColor = Color.Black;
            skc.Dock = DockStyle.Fill;
            skc.Location = new Point(0, 0);
            skc.Margin = new Padding(5, 6, 5, 6);
            skc.Name = "skc";
            skc.Size = new Size(1672, 1189);
            skc.TabIndex = 1;
            skc.VSync = true;
            skc.PaintSurface += Skc_PaintSurface;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1672, 1189);
            ContextMenuStrip = contextMenuStrip1;
            Controls.Add(skc);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form_FormClosing;
            Move += Form_Move;
            Resize += Form_Resize;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem tsiNormal;
        private ToolStripMenuItem tsiProtanomaly;
        private ToolStripMenuItem tsiDeuteranomaly;
        private ToolStripMenuItem tsiTritanomaly;
        private SkiaSharp.Views.Desktop.SKGLControl skc;
    }
}
