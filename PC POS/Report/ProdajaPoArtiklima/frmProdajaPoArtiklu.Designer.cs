﻿namespace PCPOS.Report.ProdajaPoArtiklima
{
    partial class frmProdajaPoArtiklu
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource4 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.dTRpodaciTvrtkeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dSRpodaciTvrtke = new PCPOS.Dataset.DSRpodaciTvrtke();
            this.dTlisteBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dSRliste = new PCPOS.Dataset.DSRliste();
            this.dTlisteTekstBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dSRlisteTekst = new PCPOS.Dataset.DSRlisteTekst();
            this.dTsaldaKontiBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dSsaldaKonti = new PCPOS.Dataset.DSsaldaKonti();
            this.label3 = new System.Windows.Forms.Label();
            this.tdDoDatuma = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.tdOdDatuma = new System.Windows.Forms.DateTimePicker();
            this.btnTrazi = new System.Windows.Forms.Button();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSifraArtikla = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dTRpodaciTvrtkeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSRpodaciTvrtke)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTlisteBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSRliste)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTlisteTekstBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSRlisteTekst)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTsaldaKontiBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSsaldaKonti)).BeginInit();
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
            // dTlisteBindingSource
            // 
            this.dTlisteBindingSource.DataMember = "DTliste";
            this.dTlisteBindingSource.DataSource = this.dSRliste;
            // 
            // dSRliste
            // 
            this.dSRliste.DataSetName = "DSRliste";
            this.dSRliste.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dTlisteTekstBindingSource
            // 
            this.dTlisteTekstBindingSource.DataMember = "DTlisteTekst";
            this.dTlisteTekstBindingSource.DataSource = this.dSRlisteTekst;
            // 
            // dSRlisteTekst
            // 
            this.dSRlisteTekst.DataSetName = "DSRlisteTekst";
            this.dSRlisteTekst.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dTsaldaKontiBindingSource
            // 
            this.dTsaldaKontiBindingSource.DataMember = "DTsaldaKonti";
            this.dTsaldaKontiBindingSource.DataSource = this.dSsaldaKonti;
            // 
            // dSsaldaKonti
            // 
            this.dSsaldaKonti.DataSetName = "DSsaldaKonti";
            this.dSsaldaKonti.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(289, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 575;
            this.label3.Text = "Do datuma:";
            // 
            // tdDoDatuma
            // 
            this.tdDoDatuma.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            this.tdDoDatuma.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.tdDoDatuma.Location = new System.Drawing.Point(357, 42);
            this.tdDoDatuma.Name = "tdDoDatuma";
            this.tdDoDatuma.Size = new System.Drawing.Size(177, 20);
            this.tdDoDatuma.TabIndex = 574;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 573;
            this.label2.Text = "Od datuma:";
            // 
            // tdOdDatuma
            // 
            this.tdOdDatuma.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            this.tdOdDatuma.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.tdOdDatuma.Location = new System.Drawing.Point(85, 41);
            this.tdOdDatuma.Name = "tdOdDatuma";
            this.tdOdDatuma.Size = new System.Drawing.Size(167, 20);
            this.tdOdDatuma.TabIndex = 572;
            // 
            // btnTrazi
            // 
            this.btnTrazi.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnTrazi.Image = global::PCPOS.Properties.Resources._10591;
            this.btnTrazi.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTrazi.Location = new System.Drawing.Point(540, 12);
            this.btnTrazi.Name = "btnTrazi";
            this.btnTrazi.Size = new System.Drawing.Size(96, 54);
            this.btnTrazi.TabIndex = 571;
            this.btnTrazi.Text = "       Traži";
            this.btnTrazi.UseVisualStyleBackColor = true;
            this.btnTrazi.Click += new System.EventHandler(this.btnTrazi_Click);
            // 
            // reportViewer1
            // 
            this.reportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            reportDataSource1.Name = "DSpodaciTVR";
            reportDataSource1.Value = this.dTRpodaciTvrtkeBindingSource;
            reportDataSource2.Name = "DSzaTable";
            reportDataSource2.Value = this.dTlisteBindingSource;
            reportDataSource3.Name = "DStext";
            reportDataSource3.Value = this.dTlisteTekstBindingSource;
            reportDataSource4.Name = "DSsaldaKonti";
            reportDataSource4.Value = this.dTsaldaKontiBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource3);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource4);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "PCPOS.Report.ProdajaPoArtiklima.saldakonti.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(5, 81);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(904, 726);
            this.reportViewer1.TabIndex = 576;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 26);
            this.label4.TabIndex = 578;
            this.label4.Text = "Šifra partnera\r\nartikla:";
            // 
            // txtSifraArtikla
            // 
            this.txtSifraArtikla.Location = new System.Drawing.Point(85, 12);
            this.txtSifraArtikla.Name = "txtSifraArtikla";
            this.txtSifraArtikla.Size = new System.Drawing.Size(167, 20);
            this.txtSifraArtikla.TabIndex = 577;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button1.Image = global::PCPOS.Properties.Resources.Actions_application_exit_icon;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(772, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 40);
            this.button1.TabIndex = 579;
            this.button1.Text = "Izlaz      ";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmProdajaPoArtiklu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.ClientSize = new System.Drawing.Size(914, 812);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSifraArtikla);
            this.Controls.Add(this.reportViewer1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tdDoDatuma);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tdOdDatuma);
            this.Controls.Add(this.btnTrazi);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProdajaPoArtiklu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Popis prodane robe";
            this.Load += new System.EventHandler(this.frmListe_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dTRpodaciTvrtkeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSRpodaciTvrtke)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTlisteBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSRliste)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTlisteTekstBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSRlisteTekst)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dTsaldaKontiBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dSsaldaKonti)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Dataset.DSRpodaciTvrtke dSRpodaciTvrtke;
        private System.Windows.Forms.BindingSource dTRpodaciTvrtkeBindingSource;
		private Dataset.DSRlisteTekst dSRlisteTekst;
        private System.Windows.Forms.BindingSource dTlisteTekstBindingSource;
		private Dataset.DSRliste dSRliste;
        private System.Windows.Forms.BindingSource dTlisteBindingSource;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker tdDoDatuma;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker tdOdDatuma;
        private System.Windows.Forms.Button btnTrazi;
        private Dataset.DSsaldaKonti dSsaldaKonti;
        private System.Windows.Forms.BindingSource dTsaldaKontiBindingSource;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSifraArtikla;
        private System.Windows.Forms.Button button1;
    }
}