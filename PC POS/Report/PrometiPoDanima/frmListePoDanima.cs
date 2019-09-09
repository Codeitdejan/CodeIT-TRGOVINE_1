using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PCPOS.Report.PrometiPoDanima
{
    public partial class frmListePoDanima : Form
    {
        public frmListePoDanima()
        {
            InitializeComponent();
        }

        public string artikl { get; set; }
        public string godina { get; set; }
        public string broj_dokumenta { get; set; }
        public string dokumenat { get; set; }
        public string ImeForme { get; set; }
        public string blagajnik { get; set; }
        public string skladiste { get; set; }
        public string id_podgrupa { get; set; }
        public string grupa { get; set; }
        public string ducan { get; set; }
        public string kasa { get; set; }
        public string dobavljac { get; set; }
        public string tekst { get; set; }

        public bool stavke_ispis { get; set; }
        public bool pregledRacuna = false;

        public DateTime datumOD { get; set; }
        public DateTime datumDO { get; set; }

        public string _1 { get; set; }
        public string _2 { get; set; }
        public string _3 { get; set; }
        public string _4 { get; set; }
        public string _5 { get; set; }

        private DataTable DTpdvN = new DataTable();
        private DataTable DTpdv = new DataTable();
        private DataTable DTartikli = new DataTable();

        private DataRow RowPdv;

        private decimal UG_sve = 0;
        private decimal UK_sve = 0;
        private decimal UV_sve = 0;
        private decimal UO_sve = 0;

        private int RecLineChars;
        private string ttekst;

        private ReportParameter p1;
        private ReportParameter p2;
        private ReportParameter p3;
        private ReportParameter p4;
        private ReportParameter p5;


        private void frmListe_Load(object sender, EventArgs e)
        {
            int heigt = SystemInformation.VirtualScreen.Height;
            this.Height = heigt - 60;
            this.Location = new Point((SystemInformation.VirtualScreen.Width / 2) - 411, 5);
            //DTartikli = dSRliste.Tables[0];

            this.reportViewer1.RefreshReport();

            promjenaCijene();
            preview();

            this.reportViewer1.RefreshReport();
        }

        private void promjenaCijene()
        {
            string sql1 = @"SELECT podaci_tvrtka.ime_tvrtke, podaci_tvrtka.skraceno_ime, podaci_tvrtka.oib,
podaci_tvrtka.tel, podaci_tvrtka.fax, podaci_tvrtka.mob, podaci_tvrtka.iban, podaci_tvrtka.adresa, podaci_tvrtka.vl,
podaci_tvrtka.poslovnica_adresa, podaci_tvrtka.poslovnica_grad, podaci_tvrtka.email, podaci_tvrtka.naziv_fakture,
podaci_tvrtka.text_bottom, grad.grad + '' + grad.posta AS grad
FROM podaci_tvrtka
LEFT JOIN grad ON grad.id_grad = podaci_tvrtka.id_grad;";
            classSQL.CeAdatpter(sql1).Fill(dSRpodaciTvrtke, "DTRpodaciTvrtke");
        }

        private DataRow RowOsnovica;
        private DataRow RowArtikl;

        private void StopePDVa(decimal pdv, decimal pdv_stavka, decimal osnovica, decimal iznos_ukupno)
        {
            DataRow[] dataROW = DTpdv.Select("stopa = '" + Convert.ToInt16(pdv).ToString() + "'");

            if (dataROW.Count() == 0)
            {
                RowPdv = DTpdv.NewRow();
                RowPdv["stopa"] = Convert.ToInt16(pdv).ToString();
                RowPdv["iznos"] = pdv_stavka.ToString();
                RowPdv["osnovica"] = osnovica.ToString();
                RowPdv["iznos_ukupno"] = iznos_ukupno.ToString();
                DTpdv.Rows.Add(RowPdv);
            }
            else
            {
                dataROW[0]["iznos"] = Convert.ToDecimal(dataROW[0]["iznos"].ToString()) + pdv_stavka;
                dataROW[0]["osnovica"] = Convert.ToDecimal(dataROW[0]["osnovica"].ToString()) + osnovica;
                dataROW[0]["iznos_ukupno"] = Convert.ToDecimal(dataROW[0]["iznos_ukupno"].ToString()) + iznos_ukupno;
            }
        }

        private void Artikli(string datum, string odDO, decimal osnovica, decimal pdv, decimal mpc, decimal gotovina, decimal kartice, decimal ostalo, decimal rabat, decimal povratna)
        {
            
            DataRow[] dataROW = dSRliste.Tables[0].Select("sifra = '" + datum + "'");

            if (dataROW.Count() == 0)
            {
                RowArtikl = dSRliste.Tables[0].NewRow();
                RowArtikl["sifra"] = datum;
                RowArtikl["naziv"] = odDO;

                RowArtikl["cijena1"] = Math.Round(osnovica,6).ToString("#0.00");
                RowArtikl["cijena2"] = Math.Round(pdv, 6).ToString("#0.00");
                RowArtikl["cijena5"] = Math.Round(mpc, 6).ToString("#0.00"); ;
                RowArtikl["cijena6"] = Math.Round(gotovina, 6).ToString("#0.00"); ;
                RowArtikl["cijena7"] = Math.Round(kartice, 6).ToString("#0.00"); ;
                RowArtikl["cijena9"] = Math.Round(ostalo, 6).ToString("#0.00"); ;
                RowArtikl["rabat1"] = Math.Round(rabat, 6).ToString("#0.0000");
                RowArtikl["povratna"] = Math.Round(povratna, 2).ToString("#0.00");
                dSRliste.Tables[0].Rows.Add(RowArtikl);
            }
            else
            {
                dataROW[0]["naziv"] = odDO;

                dataROW[0]["cijena1"] = Math.Round((Convert.ToDecimal(dataROW[0]["cijena1"].ToString()) + osnovica), 6).ToString("#0.00"); ;
                dataROW[0]["cijena2"] = Math.Round((Convert.ToDecimal(dataROW[0]["cijena2"].ToString()) + pdv), 6).ToString("#0.00"); ;
                dataROW[0]["cijena5"] = Math.Round((Convert.ToDecimal(dataROW[0]["cijena5"].ToString()) + mpc), 6).ToString("#0.00"); ;
                dataROW[0]["cijena6"] = Math.Round((Convert.ToDecimal(dataROW[0]["cijena6"].ToString()) + gotovina), 6).ToString("#0.00"); ;
                dataROW[0]["cijena7"] = Math.Round((Convert.ToDecimal(dataROW[0]["cijena7"].ToString()) + kartice), 6).ToString("#0.00"); ;
                dataROW[0]["cijena9"] = Math.Round((Convert.ToDecimal(dataROW[0]["cijena9"].ToString()) + ostalo), 6).ToString("#0.00"); ;
                dataROW[0]["rabat1"] = Math.Round((Convert.ToDecimal(dataROW[0]["rabat1"].ToString()) + rabat), 6).ToString("#0.0000"); ;
                dataROW[0]["povratna"] = Math.Round((Convert.ToDecimal(dataROW[0]["povratna"].ToString()) + povratna), 2).ToString("#0.00"); ;
            }
        }

        string provjera = "";
        decimal osnovica_ukupno = 0;
        decimal ukupnoRabat;

        private void preview()
        {
            string id_blag = Properties.Settings.Default.id_zaposlenik;
            DataTable DTblagajnik = classSQL.select(string.Format("Select ime, prezime from Zaposlenici where id_zaposlenik = '{0}';", id_blag), "blagajnik").Tables[0];
            string blagajnik_ = DTblagajnik.Rows[0]["ime"].ToString() + " " + DTblagajnik.Rows[0]["prezime"].ToString();
            DateTime[] datumi = new DateTime[2];
            datumi[0] = datumOD;
            datumi[1] = datumDO;

            string ducR = "";//, ducA = "";
            if (ducan != null)
            {
                ducR = string.Format(" AND racuni.id_ducan = '{0}'", ducan);
                //ducA = string.Format(" AND a.id_ducan = '{0}'", ducan);
            }

            string napR = "";
            if (kasa != null)
            {
                napR = string.Format(" AND racuni.id_kasa = '{0}'", kasa);
            }

            string blagR = "", blagA = "";
            if (blagajnik != null)
            {
                blagR = string.Format(" AND racuni.id_blagajnik = '{0}'", blagajnik);
                blagA = string.Format(" AND a.id_zaposlenik = '{0}'", blagajnik);
            }

            string grR = "";
            if (grupa != null)
            {
                grR = string.Format(" AND roba.id_grupa = '{0}'", grupa);
            }

            string dobR = "", dobA = "";
            if (dobavljac != null)
            {
                dobR = string.Format(" AND roba.id_partner = '{0}'", dobavljac);
                dobA = string.Format(" AND a.id_partner = '{0}'", dobavljac);
            }

            string odDO = null;
            string sOdDo = string.Format(@"select min(racuni.broj_racuna::integer)::text || ' - ' || max(racuni.broj_racuna::integer)::text as oddo
FROM racun_stavke
LEFT JOIN racuni ON racun_stavke.broj_racuna=racuni.broj_racuna AND racuni.id_ducan=racun_stavke.id_ducan AND racuni.id_kasa=racun_stavke.id_kasa
LEFT JOIN roba ON roba.sifra=racun_stavke.sifra_robe where racuni.datum_racuna >= '{0}' AND racuni.datum_racuna <= '{1}' {2}{3}{4}{5}{6};", datumOD, datumDO, blagR, ducR, napR, dobR, grR);

            DataSet dsOdDo = classSQL.select(sOdDo, "racuni");
            if (dsOdDo != null && dsOdDo.Tables.Count > 0 && dsOdDo.Tables[0] != null && dsOdDo.Tables[0].Rows.Count > 0 && dsOdDo.Tables[0].Rows[0][0].ToString().Length > 0)
            {
                odDO = string.Format("OD-DO računa: {0}", dsOdDo.Tables[0].Rows[0][0].ToString());
            }


            decimal kolicina = 0;
            decimal povratnaNaknada = 0;
            decimal porezNaPotrosnju = 0;
            decimal pdv = 0;
            decimal mpc = 0;
            decimal mpcc = 0;
            decimal rabat = 0;
            decimal ukupnoPorezNaPotrosnju = 0;
            decimal ukupnoPdv = 0;
            decimal ukupnoSve = 0;
            
            decimal osnovica = 0;
            decimal ukupnoPovratnaNaknada = 0;

            if (DTpdv.Columns["stopa"] == null)
            {
                DataColumn dataColumn = new DataColumn();
                dataColumn.DataType = Type.GetType("System.Int32");
                dataColumn.ColumnName = "stopa";
                DTpdv.Columns.Add(dataColumn);
                DTpdv.Columns.Add("iznos");
                DTpdv.Columns.Add("osnovica");
                DTpdv.Columns.Add("iznos_ukupno");
            }

            if (DTartikli.Columns["sifra"] == null)
            {
                DTartikli.Columns.Add("sifra");
                DTartikli.Columns.Add("kolicina");
                DTartikli.Columns.Add("mpc");
                DTartikli.Columns.Add("naziv");
            }

            if (DTpdvN.Columns["stopa"] == null)
            {
                DataColumn dataColumn = new DataColumn();
                dataColumn.DataType = Type.GetType("System.Int32");
                dataColumn.ColumnName = "stopa";
                DTpdvN.Columns.Add(dataColumn);

                DTpdvN.Columns.Add("iznos");
                DTpdvN.Columns.Add("nacin");
                DTpdvN.Columns.Add("osnovica");
                DTpdvN.Columns.Add("pov_nak");

                dataColumn = new DataColumn();
                dataColumn.DataType = Type.GetType("System.Int32");
                dataColumn.ColumnName = "srt";
                DTpdvN.Columns.Add(dataColumn);
            }
            else
            {
                DTpdvN.Clear();
            }
            string avansi = "";


            string sql_stv = string.Format(@"SELECT racun_stavke.sifra_robe, racuni.datum_racuna , racuni.broj_racuna, racun_stavke.ukupno_mpc_rabat as ukupno, roba.naziv,
Round(racun_stavke.vpc, 3) as vpc, racun_stavke.mpc, racun_stavke.porez,
SUM(CAST(REPLACE(racun_stavke.kolicina,',','.') AS NUMERIC)) AS kolicina, racun_stavke.rabat, racuni.nacin_placanja,
sum(racun_stavke.povratna_naknada) as povratna_naknada, roba.mpc AS cijena
FROM racun_stavke
LEFT JOIN racuni ON racun_stavke.broj_racuna=racuni.broj_racuna AND racuni.id_ducan=racun_stavke.id_ducan AND racuni.id_kasa=racun_stavke.id_kasa
LEFT JOIN roba ON roba.sifra=racun_stavke.sifra_robe
WHERE racuni.datum_racuna >= '{0}' AND racuni.datum_racuna <= '{1}'
{2} {3} {4} {5} {6}
GROUP BY racuni.datum_racuna,racuni.broj_racuna, racun_stavke.sifra_robe, roba.naziv, Round(racun_stavke.vpc, 3), racun_stavke.mpc, racun_stavke.porez, racun_stavke.rabat, roba.mpc, racun_stavke.ukupno_mpc_rabat, racuni.nacin_placanja
{7};",
                datumOD.ToString("yyyy-MM-dd HH:mm:ss"),
                datumDO.ToString("yyyy-MM-dd HH:mm:ss"),
                blagR,
                ducR,
                napR,
                dobR,
                grR,
                avansi);

            DataTable DT1 = classSQL.select(sql_stv, "racun_stavke").Tables[0];

            decimal osnovicaUkupno = 0, pdvUkupno = 0, ukupnoUkupno=0, UGUkupno=0, UKUKupno=0, UOUkupno=0, rabatUkupno=0, povratnaUkupno=0;
            string prvi = "", oddo, broj = "", date = datumOD.ToString("dd.MM.yyyy");
            int brojac=0;
            foreach (DataRow row in DT1.Rows)
            {
                brojac++;
                povratnaNaknada = Convert.ToDecimal(row["povratna_naknada"].ToString());
                kolicina = Convert.ToDecimal(row["kolicina"].ToString());
                mpc = Convert.ToDecimal(row["mpc"].ToString()) - (povratnaNaknada == 0 ? povratnaNaknada : (kolicina == 0 ? 0 : povratnaNaknada / kolicina));
                mpcc = Convert.ToDecimal(row["mpc"].ToString());
                pdv = Convert.ToDecimal(row["porez"].ToString());
                rabat = Convert.ToDecimal(row["rabat"].ToString());
                string brojRacuna = row["broj_racuna"].ToString();

                if (brojac == 1)
                    prvi = brojRacuna;

                decimal rabatIznos = Math.Round((mpc * (rabat / 100)), 6, MidpointRounding.AwayFromZero);
                decimal mpc_s_rab = Math.Round(mpc - rabatIznos, 6, MidpointRounding.AwayFromZero);
                decimal mpcSRabUkupno = Math.Round((mpc_s_rab * kolicina), 6, MidpointRounding.AwayFromZero);
                decimal rabatt = mpcc * (rabat / 100) * kolicina;

                ukupnoRabat += rabatt;

                //Ovaj kod dobiva PDV
                decimal PreracunataStopaPDV = Convert.ToDecimal((100 * pdv) / (100 + pdv + porezNaPotrosnju));
                decimal pdvIznos = Math.Round(((mpc_s_rab * (Math.Round(PreracunataStopaPDV, 6, MidpointRounding.AwayFromZero) / 100)) * kolicina), 6, MidpointRounding.AwayFromZero);
                ukupnoPdv = pdvIznos + ukupnoPdv;

                ukupnoPovratnaNaknada += povratnaNaknada;

                //Ovaj kod dobiva porez na potrošnju
                decimal PreracunataStopaPorezNaPotrosnju = Convert.ToDecimal((100 * porezNaPotrosnju) / (100 + pdv + porezNaPotrosnju));
                decimal porezNaPotrosnjuIznos = Math.Round(((mpc_s_rab * (Math.Round(PreracunataStopaPorezNaPotrosnju, 6, MidpointRounding.AwayFromZero) / 100)) * kolicina), 6, MidpointRounding.AwayFromZero);
                ukupnoPorezNaPotrosnju = porezNaPotrosnjuIznos + ukupnoPorezNaPotrosnju;

                ukupnoSve = mpcSRabUkupno + ukupnoSve;

                decimal UG = 0;
                decimal UK = 0;
                decimal UV = 0;
                decimal UO = 0;

                int sort = 0;
                if (row["nacin_placanja"].ToString() == "G")
                {
                    sort = 1;
                    UG = mpcSRabUkupno;
                    UG_sve += UG;
                }
                else if (row["nacin_placanja"].ToString() == "K")
                {
                    sort = 2;
                    UK = mpcSRabUkupno;
                    UK_sve += UK;
                }
                else if (row["nacin_placanja"].ToString() == "T")
                {
                    sort = 3;
                    UV = mpcSRabUkupno;
                    UV_sve += UV;
                }
                else if (row["nacin_placanja"].ToString() == "O")
                {
                    sort = 4;
                    UO = mpcSRabUkupno;
                    UO_sve += UO;
                }
                decimal osnovicauk = (mpcSRabUkupno - ((pdvIznos) + (porezNaPotrosnjuIznos)));
                StopePDVaN(pdv, pdvIznos, row["nacin_placanja"].ToString(), osnovicauk, povratnaNaknada, sort);
                StopePDVa(pdv, pdvIznos, osnovicauk, mpc_s_rab);
                
                osnovica_ukupno = osnovicauk + osnovica_ukupno;

                DateTime d = Convert.ToDateTime(row["datum_racuna"].ToString());
                decimal ukupno = osnovicauk + pdvIznos + povratnaNaknada;
                string dt = d.ToString("dd.MM.yyyy");

                
                

                if (date != dt)
                {
                    
                    oddo = prvi + " - " + (int.Parse(brojRacuna)-1);
                    osnovicaUkupno += osnovicauk;
                    pdvUkupno += pdvIznos;
                    ukupnoUkupno += ukupno;
                    UGUkupno += UG;
                    UKUKupno += UK;
                    UOUkupno += UO;
                    rabatUkupno += rabatt;
                    povratnaUkupno += povratnaNaknada;

                    prvi = brojRacuna;
                    Artikli(date, oddo, osnovicaUkupno, pdvUkupno, ukupnoUkupno, UGUkupno, UKUKupno, UOUkupno, rabatUkupno, povratnaUkupno);
                    date = dt;

                    osnovicaUkupno = 0;
                    pdvUkupno = 0;
                    ukupnoUkupno = 0;
                    UGUkupno = 0;
                    UKUKupno = 0;
                    UOUkupno = 0;
                    rabatUkupno = 0;
                    povratnaUkupno = 0;
                }
                else if (DT1.Rows.Count == brojac)
                {
                    oddo = prvi + " - " + brojRacuna;

                    osnovicaUkupno += osnovicauk;
                    pdvUkupno += pdvIznos;
                    ukupnoUkupno += ukupno;
                    UGUkupno += UG;
                    UKUKupno += UK;
                    UOUkupno += UO;
                    rabatUkupno += rabatt;
                    povratnaUkupno += povratnaNaknada;

                    prvi = brojRacuna;
                    Artikli(date, oddo, osnovicaUkupno, pdvUkupno, ukupnoUkupno, UGUkupno, UKUKupno, UOUkupno, rabatUkupno, povratnaUkupno);
                    date = dt;

                    osnovicaUkupno = 0;
                    pdvUkupno = 0;
                    ukupnoUkupno = 0;
                    UGUkupno = 0;
                    UKUKupno = 0;
                    UOUkupno = 0;
                    rabatUkupno = 0;
                    povratnaUkupno = 0;
                }
                else
                {
                    osnovicaUkupno += osnovicauk;
                    pdvUkupno += pdvIznos;
                    ukupnoUkupno += ukupno;
                    UGUkupno += UG;
                    UKUKupno += UK;
                    UOUkupno += UO;
                    rabatUkupno += rabatt;
                    povratnaUkupno += povratnaNaknada;
                }
            }
            
            string porezi = "";

            DataView dv = DTpdvN.DefaultView;
            dv.Sort = "srt, stopa";
            DTpdvN = dv.ToTable();

            for (int i = 0; i < DTpdvN.Rows.Count; i++)
            {
                //if (Convert.ToDecimal(DTpdvN.Rows[i]["stopa"].ToString()) > 0 || !Class.Postavke.sustavPdv)
                if (true)
                {
                    string nacin_pplacanja = "";

                    decimal ispisOsnovica = Math.Round(Convert.ToDecimal(DTpdvN.Rows[i]["osnovica"].ToString()), 2, MidpointRounding.AwayFromZero);
                    decimal ispisPorez = Math.Round(Convert.ToDecimal(DTpdvN.Rows[i]["iznos"].ToString()), 2, MidpointRounding.AwayFromZero);
                    decimal iznosPovratnaNaknada = Math.Round(Convert.ToDecimal(DTpdvN.Rows[i]["pov_nak"].ToString()), 2, MidpointRounding.AwayFromZero);

                    decimal ukupno_za_nacin_pl = ispisPorez + ispisOsnovica + iznosPovratnaNaknada;

                    if (DTpdvN.Rows[i]["nacin"].ToString().ToUpper() == "G")
                    {
                        nacin_pplacanja = "GOTOVINA";// +UG_sve + " kn";
                    }
                    else if (DTpdvN.Rows[i]["nacin"].ToString().ToUpper() == "T")
                    {
                        nacin_pplacanja = "TRANSAKCIJSKI RAČUN";// + UV_sve + " kn";
                    }
                    else if (DTpdvN.Rows[i]["nacin"].ToString().ToUpper() == "O")
                    {
                        nacin_pplacanja = "OSTALO";// + UO_sve + " kn";
                    }
                    else if (DTpdvN.Rows[i]["nacin"].ToString().ToUpper() == "K")
                    {
                        nacin_pplacanja = "KARTICE";// + UK_sve + " kn";
                    }
                    
                    porezi += "Način fiskaliziranja: " + nacin_pplacanja +
                        "\r\nOsnovica " + DTpdvN.Rows[i]["stopa"].ToString() + " %: " + ispisOsnovica.ToString("#0.00") + " kn" +
                        "\r\nIznos poreza: " + ispisPorez.ToString("#0.00") + " kn" +
                        "\r\nPovratna naknada: " + iznosPovratnaNaknada.ToString("#0.00") + " kn" +
                        "\r\nUkupno za " + nacin_pplacanja.ToLower() + ": " + ukupno_za_nacin_pl.ToString("#0.00") + " kn" +
                        "\r\n\r\n";
                }
            }
            dv = null;

            p1 = new ReportParameter("datum", "Od datuma: " + datumOD.ToString("dd.MM.yyyy") + " do datuma " + datumDO.ToString("dd.MM.yyyy"));
            p2 = new ReportParameter("stope_poreza", porezi);
            p3 = new ReportParameter("osnovica", osnovica_ukupno.ToString("#0.00"));
            p4 = new ReportParameter("pdv", ukupnoPdv.ToString("#0.00"));
            p5 = new ReportParameter("rabat", ukupnoRabat.ToString("#0.00"));


            this.reportViewer1.LocalReport.EnableExternalImages = true;
            this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5});
            //porezi += Environment.NewLine + "Povratna naknada: " + ukupnoPovratnaNaknada.ToString("#0.00") + " kn";

            int a = Class.PosPrint.ispredArtikla;
            int k = Class.PosPrint.ispredKolicine;
            int c = Class.PosPrint.ispredCijene;
            int pp = Class.PosPrint.ispredPopusta;
            int s = Class.PosPrint.ispredUkupno;

            int pp_alt = pp + 2;
            int k_alt = k + 2;
            int s_alt = s + 2;
            int c_alt = c + 2;

            RecLineChars = a + k_alt + c_alt + pp_alt + s_alt;

            //Dodao 24.10.2013

            string naziv_artika = "";
            //string sifra_artikla = "";
            decimal kol_artikla = 0;
            decimal rabat_postotak_artikla = 0;
            decimal mpc_artikla = 0;
            decimal ukupno_artikl = 0;
            decimal ukupno_po_rac = 0;
            decimal ukupno_rabat = 0;

            foreach (DataRow row1 in DT1.Rows)
            {
                decimal mpc_sa_rabatom = 0;
                naziv_artika = row1["naziv"].ToString();
                kol_artikla = Convert.ToDecimal(row1["kolicina"].ToString());
                mpc_artikla = Convert.ToDecimal(row1["mpc"].ToString());
                rabat_postotak_artikla = Convert.ToDecimal(row1["rabat"].ToString());
                mpc_sa_rabatom = mpc_artikla - (mpc_artikla * (rabat_postotak_artikla / 100));
                ukupno_artikl = mpc_sa_rabatom * kol_artikla;
                ukupno_rabat += mpc_artikla * (rabat_postotak_artikla / 100) * kol_artikla;
                // Dodano za apsolutne popuste
                if (row1[0].ToString() == "!popustABS") ukupno_rabat -= Convert.ToDecimal(row1["mpc"].ToString()) * kol_artikla;
                ukupno_po_rac += mpc_sa_rabatom * kol_artikla;

            }

            if (stavke_ispis)
            {

            }
            else
            {
                ttekst += "\r\n\r\n";
            }
            //Dodao 24.10.2013

            ttekst += "RAZVRSTANO PO POREZIMA\r\n";

            dv = null;
            dv = DTpdv.DefaultView;
            dv.Sort = "stopa";
            DTpdv = dv.ToTable();
            for (int i = 0; i < DTpdv.Rows.Count; i++)
            {
                string Tpdv = Math.Round(Convert.ToDecimal(DTpdv.Rows[i]["iznos"].ToString()), 2, MidpointRounding.AwayFromZero).ToString("#0.00") + " kn";
                string Tosnovica = Math.Round(Convert.ToDecimal(DTpdv.Rows[i]["osnovica"].ToString()), 2, MidpointRounding.AwayFromZero).ToString("#0.00") + " kn";

            }

            dv = null;

            ttekst += "POVRATNA NAKNADA: " + Math.Round(ukupnoPovratnaNaknada, 2, MidpointRounding.AwayFromZero).ToString("#0.00") + " kn" + "\r\n";
            ttekst += "UKUPNO BEZ POV.NAK.: " + Math.Round(ukupnoSve, 2, MidpointRounding.AwayFromZero).ToString("#0.00") + " kn" + "\r\n";
            ttekst += "RABAT  : " + Math.Round(ukupno_rabat, 2, MidpointRounding.AwayFromZero).ToString("#0.00") + " kn" + "\r\n";
            ttekst += "UKUPNO S POV.NAK.: " + Math.Round(ukupnoSve + ukupnoPovratnaNaknada, 2, MidpointRounding.AwayFromZero).ToString("#0.00") + " kn" + "\r\n";
            if (Class.PodaciTvrtka.oibTvrtke == "67660751355") { ttekst += "UKUPNO BEZ RABATA: " + Math.Round(ukupnoSve + ukupno_rabat, 2, MidpointRounding.AwayFromZero).ToString("#0.00") + " kn"; }

            ttekst += "\r\n\r\n";

            ttekst += "DETALJNO PO NAČINIMA FISKALIZIRANJA:\r\n" + porezi;


        }


        private void StopePDVaN(decimal pdv, decimal pdv_stavka, string nacin_P, decimal osnovica, decimal pov_nak = 0, int srt = 0)
            {
                DataRow[] dataROW = DTpdvN.Select("stopa = '" + Convert.ToInt16(pdv).ToString() + "' AND nacin='" + nacin_P + "'");

                if (dataROW.Count() == 0)
                {
                    RowPdv = DTpdvN.NewRow();
                    RowPdv["stopa"] = Convert.ToInt16(pdv).ToString();
                    RowPdv["iznos"] = pdv_stavka.ToString();
                    RowPdv["nacin"] = nacin_P;
                    RowPdv["osnovica"] = osnovica;
                    RowPdv["pov_nak"] = pov_nak;
                    RowPdv["srt"] = srt;

                    DTpdvN.Rows.Add(RowPdv);
                }
                else
                {
                    dataROW[0]["iznos"] = Convert.ToDecimal(dataROW[0]["iznos"].ToString()) + pdv_stavka;
                    dataROW[0]["osnovica"] = Convert.ToDecimal(dataROW[0]["osnovica"].ToString()) + osnovica;
                    dataROW[0]["pov_nak"] = Convert.ToDecimal(dataROW[0]["pov_nak"].ToString()) + pov_nak;
                }
            }

        }
    }
