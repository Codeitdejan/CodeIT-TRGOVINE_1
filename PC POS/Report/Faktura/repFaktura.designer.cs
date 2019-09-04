namespace PCPOS.Report.Faktura
{
    partial class repFaktura
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dTRpodaciTvrtkeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dSRpodaciTvrtke = new PCPOS.Dataset.DSRpodaciTvrtke();
            this.dTfakturaStavkeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dSRfakturaStavke = new PCPOS.Dataset.DSRfakturaStavke();
            this.dTstopeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dSstope = new PCPOS.Dataset.DSstope();
            this.dTRfakturaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dSFaktura = new PCPOS.Dataset.DSFaktura();
            this.dSavansiFakBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet1 = new PCPOS.Dataset.DataSet1();
            this.bindingSourcePdfCode = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetPdf417Code = new PCPOS.Dataset.DataSetPdf417Code();
            ((System.ComponentModel.ISupportInitialize)(this.dTRpodaciTvrtkeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSRpodaciTvrtke)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTfakturaStavkeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSRfakturaStavke)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTstopeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSstope)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTRfakturaBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSFaktura)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSavansiFakBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePdfCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetPdf417Code)).BeginInit();
            this.SuspendLayout();
            // 
            // dTRpodaciTvrtkeBindingSource
            // 
            this.dTRpodaciTvrtkeBindingSource.DataMember = "DTRpodaciTvrtke";
            this.dTRpodaciTvrtkeBindingSource.DataSource = this.dSRpodaciTvrtke;
            // 
            // dSRpodaciTvrtke
            // 
            this.dSRpodaciTvrtke.DataSetName = "DSRpodaciTvrtke";
            this.dSRpodaciTvrtke.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dTfakturaStavkeBindingSource
            // 
            this.dTfakturaStavkeBindingSource.DataMember = "DTfakturaStavke";
            this.dTfakturaStavkeBindingSource.DataSource = this.dSRfakturaStavke;
            // 
            // dSRfakturaStavke
            // 
            this.dSRfakturaStavke.DataSetName = "DSRfakturaStavke";
            this.dSRfakturaStavke.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dTstopeBindingSource
            // 
            this.dTstopeBindingSource.DataMember = "DTstope";
            this.dTstopeBindingSource.DataSource = this.dSstope;
            // 
            // dSstope
            // 
            this.dSstope.DataSetName = "DSstope";
            this.dSstope.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dTRfakturaBindingSource
            // 
            this.dTRfakturaBindingSource.DataMember = "DTRfaktura";
            this.dTRfakturaBindingSource.DataSource = this.dSFaktura;
            // 
            // dSFaktura
            // 
            this.dSFaktura.DataSetName = "DSFaktura";
            this.dSFaktura.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dSavansiFakBindingSource
            // 
            this.dSavansiFakBindingSource.DataMember = "DSavansiFak";
            this.dSavansiFakBindingSource.DataSource = this.dataSet1;
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "DataSet1";
            this.dataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bindingSourcePdfCode
            // 
            this.bindingSourcePdfCode.DataMember = "PDF417";
            this.bindingSourcePdfCode.DataSource = this.dataSetPdf417Code;
            // 
            // dataSetPdf417Code
            // 
            this.dataSetPdf417Code.DataSetName = "DataSetPdf417Code";
            this.dataSetPdf417Code.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // repFaktura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 775);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "repFaktura";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Faktura ";
            this.Load += new System.EventHandler(this.repFaktura_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dTRpodaciTvrtkeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSRpodaciTvrtke)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTfakturaStavkeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSRfakturaStavke)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTstopeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSstope)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTRfakturaBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSFaktura)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSavansiFakBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePdfCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetPdf417Code)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

		public Dataset.DSFaktura dSFaktura;
		public System.Windows.Forms.BindingSource dTRfakturaBindingSource;
		public Dataset.DSRfakturaStavke dSRfakturaStavke;
		public System.Windows.Forms.BindingSource dTfakturaStavkeBindingSource;
		public Dataset.DSRpodaciTvrtke dSRpodaciTvrtke;
		public System.Windows.Forms.BindingSource dTRpodaciTvrtkeBindingSource;
		public Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private Dataset.DSstope dSstope;
        private System.Windows.Forms.BindingSource dTstopeBindingSource;
        private Dataset.DataSet1 dataSet1;
        private System.Windows.Forms.BindingSource dSavansiFakBindingSource;
        private System.Windows.Forms.BindingSource bindingSourcePdfCode;
        private Dataset.DataSetPdf417Code dataSetPdf417Code;
    }
}