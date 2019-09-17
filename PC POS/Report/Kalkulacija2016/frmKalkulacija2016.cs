﻿using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PCPOS.Report.Kalkulacija
{
    public partial class frmKalkulacija2016 : Form
    {
        public frmKalkulacija2016()
        {
            InitializeComponent();
        }

        public string broj_kalkulacije { get; set; }
        public string skladiste { get; set; }
        public bool pregled { get; set; }
        public DataRow RowPdv { get; set; }

        private string _dokument = "kalkulacija";
        public string dokument { get { return _dokument; } set { _dokument = value; } }
        private INIFile ini = new INIFile();

        decimal nbcPdvUkupno = 0;

        private void frmKalkulacija_Load(object sender, EventArgs e)
        {
            int heigt = SystemInformation.VirtualScreen.Height;
            this.Height = heigt - 60;
            this.Location = new Point((SystemInformation.VirtualScreen.Width / 2) - 411, 5);
            this.reportViewer2.SetDisplayMode(DisplayMode.PrintLayout);
            if (dokument == "kalkulacija")
            {
                SetKalkulacija();
            }
            else if (dokument == "povrat_dobavljacu")
            {
                SetPovratDobavljacu();
            }

            string marza = "0";
            try
            {
                if (ini.Read("POSTAVKE", "inventura_nabavna") == "1")
                {
                    marza = "1";
                }
            }
            catch { }

            ReportParameter p1 = new ReportParameter("marza", marza);
            this.reportViewer2.LocalReport.EnableExternalImages = true;
            this.reportViewer2.LocalReport.SetParameters(new ReportParameter[] { p1 });
            this.reportViewer2.RefreshReport();
        }

        private void SetPovratDobavljacu()
        {
            string filter = "";
            if (pregled == true)
            {
                filter = "kalkulacija.id_kalkulacija='" + broj_kalkulacije + "';";
            }
            else
            {
                filter = "povrat_robe.broj='" + broj_kalkulacije + "' AND povrat_robe.id_skladiste='" + skladiste + "';";
            }

            string SustavPdv = @"select * from partners where id_partner = (select id_partner from povrat_robe where broj = '" + broj_kalkulacije + "')";

            DataSet dsPartner = classSQL.select(SustavPdv, "partners");
            bool partnerExist = false, uSustavuPdv = false;
            if (dsPartner != null && dsPartner.Tables.Count > 0 && dsPartner.Tables[0] != null && dsPartner.Tables[0].Rows.Count > 0)
            {
                partnerExist = true;
                uSustavuPdv = Convert.ToBoolean(dsPartner.Tables[0].Rows[0]["uSustavPdv"].ToString());
            }

            string sqlTecaj = @"SELECT povrat_robe.broj, povrat_robe.godina, skladiste.skladiste, povrat_robe.tecaj, valute.ime_valute AS valuta
FROM povrat_robe
LEFT JOIN skladiste ON povrat_robe.id_skladiste=skladiste.id_skladiste
LEFT JOIN valute ON povrat_robe.id_valuta=valute.id_valuta
LEFT JOIN partners ON povrat_robe.id_partner=partners.id_partner
WHERE " + filter + " ";

            DataTable dtTecaj = classSQL.select(sqlTecaj, "kalkulacija").Tables[0];

            double tecaj = 1;

            if (dtTecaj.Rows.Count > 0)
            {
                tecaj = Convert.ToDouble(dtTecaj.Rows[0]["tecaj"].ToString());
            }

            string naziv_fakture = " podaci_tvrtka.naziv_fakture,";
            string sql1 = SqlPodaciTvrtke.VratiSql("", naziv_fakture, "");

            if (classSQL.remoteConnectionString == "")
            {
                classSQL.CeAdatpter(sql1).Fill(dSRpodaciTvrtke, "DTRpodaciTvrtke");
            }
            else
            {
                //classSQL.NpgAdatpter(sql1).Fill(dSRpodaciTvrtke, "DTRpodaciTvrtke");
                classSQL.CeAdatpter(sql1).Fill(dSRpodaciTvrtke, "DTRpodaciTvrtke");
            }

            DataTable dtPdv = new DataTable();
            dtPdv.Columns.Add("stopa");
            dtPdv.Columns.Add("iznos");
            dtPdv.Columns.Add("osnovica");

            string filter1 = "";
            if (pregled == true)
            {
                filter1 = "povrat_robe_stavke.id_kalkulacija='" + broj_kalkulacije + "'";
            }
            else
            {
                filter1 = "povrat_robe_stavke.broj='" + broj_kalkulacije + "' AND povrat_robe_stavke.id_skladiste='" + skladiste + "'";
            }

            string sql_stavke = "SELECT " +
                " povrat_robe_stavke.sifra," +
                " povrat_robe_stavke.id_stavka AS id," +
                " roba.naziv," +
                " roba.jm AS jmj," +
                " povrat_robe_stavke.kolicina," +
                " povrat_robe_stavke.rabat," +
                " povrat_robe_stavke.nbc," +
                " 0 as prijevoz," +
                " 0 as carina," +
                " 0 as posebni_porez," +
                " round(((povrat_robe_stavke.vpc / povrat_robe_stavke.nbc::numeric) - 1) * 100, 6) as marza," +
                " povrat_robe_stavke.vpc," +
                " povrat_robe_stavke.pdv AS pdv" +
                " FROM povrat_robe_stavke" +
                " LEFT JOIN roba ON povrat_robe_stavke.sifra=roba.sifra" +
                " WHERE " + filter1 + " ORDER BY CAST(id_stavka AS INT) ASC";

            if (classSQL.remoteConnectionString == "")
            {
                classSQL.CeAdatpter(sql_stavke).Fill(dSkalkulacija_stavke, "DTkalkDtavke");
            }
            else
            {
                classSQL.NpgAdatpter(sql_stavke).Fill(dSkalkulacija_stavke, "DTkalkDtavke");
            }

            DataTable DT = dSkalkulacija_stavke.Tables[0];
            double fakIznos = 0;
            double fak_iznos_stavka = 0;
            double fakNetto = 0;
            double osnovica = 0;
            double fakPdv = 0;
            double fak_ukupno_stavka = 0;
            double fakUk = 0;
            double fak_cijena = 0;
            double kolicina = 0;
            double rabat = 0;
            double rabatStavka = 0;
            double rabatUk = 0;
            double mpc = 0;
            double mpc_uk = 0;
            double porez = 0;
            double vpc = 0;
            double nab_cijena = 0;
            double posebni_porez = 0;
            double prijevoz = 0;
            double carina = 0;
            double net_cijena = 0;
            double marza = 0;
            double marzaIznos = 0;
            double marzaUk = 0;
            double nabavna_zadnja = 0;
            double marza_uk_ = 0;
            double fak_cijena_S_pdv = 0;
            double fak_cijena_S_pdv_ukp = 0;

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                kolicina = Math.Round(Convert.ToDouble(DT.Rows[i]["kolicina"].ToString()), 3);
                fak_cijena = Math.Round(Convert.ToDouble(DT.Rows[i]["fak_cijena"].ToString()), 3);
                rabat = Math.Round(Convert.ToDouble(DT.Rows[i]["rabat"].ToString()), 3);
                porez = Math.Round(Convert.ToDouble(DT.Rows[i]["pdv"].ToString()), 3);
                vpc = Math.Round(Convert.ToDouble(DT.Rows[i]["vpc"].ToString()), 3);
                posebni_porez = Math.Round(Convert.ToDouble(DT.Rows[i]["posebni_porez"].ToString()), 3);
                prijevoz = Math.Round(Convert.ToDouble(DT.Rows[i]["prijevoz"].ToString()), 4);
                carina = Math.Round(Convert.ToDouble(DT.Rows[i]["carina"].ToString()), 3);
                marza = Math.Round(Convert.ToDouble(DT.Rows[i]["marza"].ToString()), 8);

                if (marza.ToString() == "NaN" || marza.ToString() == "Infinity")
                    marza = 0;
                double vpc_s_kol = vpc * kolicina;
                double fak_s_kol = fak_cijena * kolicina;
                nabavna_zadnja = Math.Round((fak_s_kol - (fak_s_kol * (rabat / 100)) + ((prijevoz + carina) * kolicina)), 3);
                marza_uk_ = Math.Round(nabavna_zadnja * (marza / 100), 3);
                marzaIznos = Math.Round(marza / 100 * nabavna_zadnja, 3);
                marzaUk += marzaIznos;

                rabatStavka = fak_cijena * kolicina * rabat / 100;
                rabatUk += rabatStavka;

                mpc = vpc * (1 + porez / 100.0);

                mpc_uk = mpc * kolicina;

                nab_cijena = kolicina * fak_cijena * (1 - rabat / 100) - posebni_porez + ((prijevoz + carina) * kolicina);
                //if (Util.Korisno.oibTvrtke == "41109922301") {
                //    nab_cijena = fak_cijena * (1 - rabat / 100) - posebni_porez + ((prijevoz + carina));
                //}

                fak_cijena_S_pdv = nab_cijena * (1 + (porez / 100));
                if (partnerExist && !uSustavuPdv)
                    fak_cijena_S_pdv = nab_cijena;

                fak_cijena_S_pdv_ukp += fak_cijena_S_pdv;

                net_cijena = kolicina * fak_cijena * (1 - rabat / 100);

                fak_iznos_stavka = kolicina * fak_cijena;
                fakIznos += fak_iznos_stavka;

                fakNetto += fak_iznos_stavka * (1 - rabat / 100);

                fak_ukupno_stavka = vpc * (1 + porez / 100) * kolicina;
                fakUk += fak_ukupno_stavka;

                osnovica += fak_ukupno_stavka * 100 / (100 + porez);
                fakPdv += fak_ukupno_stavka * (1 - 100 / (100 + porez));
                //fak_pdv += fak_ukupno_stavka / (1 - porez);

                //double stopa = ((mpc - vpc) / vpc);
                double pdvStavka_cisto = (nab_cijena) * (porez / 100);
                if (partnerExist && !uSustavuPdv)
                    pdvStavka_cisto = 0;
                StopePDVa(Math.Round((decimal)porez, 4), Math.Round((decimal)pdvStavka_cisto, 4), Math.Round(((decimal)nab_cijena), 4));

                DT.Rows[i].SetField("mpc_uk", Math.Round(mpc_uk, 3));
                DT.Rows[i].SetField("mpc", Math.Round(mpc, 3));
                DT.Rows[i].SetField("rabat", Math.Round(rabat, 3));
                DT.Rows[i].SetField("rabat_uk", Math.Round(rabatStavka, 3));
                DT.Rows[i].SetField("marza", Math.Round(marza, 3));
                DT.Rows[i].SetField("marzaIznos", Math.Round(marza_uk_, 3));
                DT.Rows[i].SetField("posebni_porez", posebni_porez);
                DT.Rows[i].SetField("prijevoz", Math.Round(prijevoz, 4));
                DT.Rows[i].SetField("carina", Math.Round(carina, 3));

                DT.Rows[i].SetField("kolicina", Math.Round(kolicina, 3));
                DT.Rows[i].SetField("fak_cijena", Math.Round(fak_cijena, 3));
                DT.Rows[i].SetField("fak_cijena_val", Math.Round(fak_cijena / tecaj, 3));
                DT.Rows[i].SetField("net_cijena", Math.Round(net_cijena, 3));
                DT.Rows[i].SetField("nab_cijena", Math.Round(nab_cijena, 3));
                DT.Rows[i].SetField("fak_cijena_S_pdv", Math.Round(fak_cijena_S_pdv, 3));

                //DT.Rows[i].SetField("nab_porez", Math.Round(nab_cijena, 3));
            }

            fakIznos = Math.Round(fakIznos, 3);
            fakNetto = Math.Round(fakNetto, 3);
            fakPdv = Math.Round(fakPdv, 3);
            osnovica = Math.Round(osnovica, 3);
            fakUk = Math.Round(fakUk, 3);
            rabatUk = Math.Round(rabatUk, 3);
            marzaUk = Math.Round(marzaUk, 3);
            string filter2 = "";
            if (pregled == true)
            {
                filter2 = "kalkulacija.id_kalkulacija='" + broj_kalkulacije + "';";
            }
            else
            {
                filter2 = "kalkulacija.broj='" + broj_kalkulacije + "' AND kalkulacija.id_skladiste='" + skladiste + "';";
            }
            string sql_kalk = @"SELECT kalkulacija.broj, kalkulacija.id_partner, kalkulacija.godina, 'Kalkulacija' AS naslov, skladiste.skladiste, kalkulacija.datum,
partners.ime_tvrtke + ' ' + partners.id_partner AS dobavljac, kalkulacija.tecaj AS tecaj, kalkulacija.racun, kalkulacija.otpremnica,
CAST(kalkulacija.otpremnica_datum AS date) AS otpremnica_datum, kalkulacija.tecaj, zaposlenici.ime + ' ' + zaposlenici.prezime AS kalkulirao,
CAST(kalkulacija.racun_datum AS date) AS racun_datum, valute.ime_valute AS valuta, '" + fakIznos + @"' AS fak_iznos, '" + fakNetto + @"' AS netto_fak_iznos,
'" + fakPdv + @"' AS pdv, '" + rabatUk + @"' AS rabatUk, '" + marzaUk + @"' AS marzaUk, '" + osnovica + @"' AS osnovica, '" + fakUk + @"' AS ukupno,
'" + Math.Round(fak_cijena_S_pdv_ukp, 2) + @"' as fak_cijena_S_pdv, case when partners.uSustavPdv = true then 'Dobavljač u sustavu PDV-a: DA' else 'Dobavljač u sustavu PDV-a: NE' end as sustavPdv
FROM kalkulacija
LEFT JOIN skladiste ON kalkulacija.id_skladiste=skladiste.id_skladiste
LEFT JOIN zaposlenici ON kalkulacija.id_zaposlenik=zaposlenici.id_zaposlenik
LEFT JOIN valute ON kalkulacija.id_valuta=valute.id_valuta
LEFT JOIN partners ON kalkulacija.id_partner=partners.id_partner
WHERE " + filter2;

            if (classSQL.remoteConnectionString == "")
            {
                classSQL.CeAdatpter(sql_kalk).Fill(dSkalkulacija, "DTKalkulacijA");
            }
            else
            {
                classSQL.NpgAdatpter(sql_kalk).Fill(dSkalkulacija, "DTKalkulacijA");
            }
        }

        /// <summary>
        /// Fills report with "Kalkulacije" data
        /// </summary>
        private void SetKalkulacija()
        {
            string filter = "", sql = "";
            if (pregled == true)
            {
                filter = "kalkulacija.id_kalkulacija='" + broj_kalkulacije + "'";
                sql = @"select coalesce(sum(pn.iznos * replace(ks.kolicina, ',','.')::numeric), 0) as povratna_naknada
from povratna_naknada pn
left join kalkulacija_stavke ks on pn.sifra = ks.sifra
left join kalkulacija k on ks.broj = k.broj and ks.id_skladiste = k.id_skladiste
where k.broj = '" + broj_kalkulacije + "' and pn.iznos <> 0;";
            }
            else
            {
                filter = "kalkulacija.broj='" + broj_kalkulacije + "' AND kalkulacija.id_skladiste='" + skladiste + "'";

                sql = @"select coalesce(sum(pn.iznos * replace(ks.kolicina, ',','.')::numeric), 0) as povratna_naknada
from povratna_naknada pn
left join kalkulacija_stavke ks on pn.sifra = ks.sifra
left join kalkulacija k on ks.broj = k.broj and ks.id_skladiste = k.id_skladiste
where k.broj = '" + broj_kalkulacije + "' and k.id_skladiste = '" + skladiste + "' and pn.iznos <> 0;";
            }

            DataSet dsPovratnaNaknada = classSQL.select(sql, "povratna_naknada");
            decimal povratna_naknada = 0;

            if (dsPovratnaNaknada != null && dsPovratnaNaknada.Tables.Count > 0 && dsPovratnaNaknada.Tables[0] != null && dsPovratnaNaknada.Tables[0].Rows.Count > 0)
                povratna_naknada = Convert.ToDecimal(dsPovratnaNaknada.Tables[0].Rows[0][0]);

            ReportParameter p2 = new ReportParameter("povratna_naknada", povratna_naknada.ToString());
            this.reportViewer2.LocalReport.SetParameters(new ReportParameter[] { p2 });

            string SustavPdv = @"select * from partners where id_partner = (select id_partner from kalkulacija where broj = '" + broj_kalkulacije + @"' and id_skladiste = '" + skladiste + "')";

            DataSet dsPartner = classSQL.select(SustavPdv, "partners");
            bool partnerExist = false, uSustavuPdv = false;
            if (dsPartner != null && dsPartner.Tables.Count > 0 && dsPartner.Tables[0] != null && dsPartner.Tables[0].Rows.Count > 0)
            {
                partnerExist = true;
                uSustavuPdv = Convert.ToBoolean(dsPartner.Tables[0].Rows[0]["uSustavPdv"].ToString());
            }

            string sqlTecaj = @"SELECT kalkulacija.broj, kalkulacija.godina, skladiste.skladiste, kalkulacija.tecaj, kalkulacija.trosak, valute.ime_valute AS valuta
FROM kalkulacija
LEFT JOIN skladiste ON kalkulacija.id_skladiste=skladiste.id_skladiste
LEFT JOIN valute ON kalkulacija.id_valuta=valute.id_valuta
LEFT JOIN partners ON kalkulacija.id_partner=partners.id_partner
WHERE " + filter + " ";

            DataTable dtTecaj = classSQL.select(sqlTecaj, "kalkulacija").Tables[0];

            double tecaj = 1;
            decimal trosak = Convert.ToDecimal(dtTecaj.Rows[0]["trosak"].ToString());

            if (dtTecaj.Rows.Count > 0)
            {
                tecaj = Convert.ToDouble(dtTecaj.Rows[0]["tecaj"].ToString());
            }

            string naziv_fakture = " podaci_tvrtka.naziv_fakture,";
            string sql1 = SqlPodaciTvrtke.VratiSql("", naziv_fakture, "");

            if (classSQL.remoteConnectionString == "")
            {
                classSQL.CeAdatpter(sql1).Fill(dSRpodaciTvrtke, "DTRpodaciTvrtke");
            }
            else
            {
                classSQL.CeAdatpter(sql1).Fill(dSRpodaciTvrtke, "DTRpodaciTvrtke");
            }

            DataTable dtPdvNbc = new DataTable();
            dtPdvNbc.Columns.Add("stopa");
            dtPdvNbc.Columns.Add("iznos");
            dtPdvNbc.Columns.Add("osnovica");

            string filter1 = "";
            if (pregled == true)
            {
                filter1 = string.Format("ks.id_kalkulacija = '{0}'", broj_kalkulacije);
            }
            else
            {
                filter1 = string.Format("ks.broj = '{0}' AND ks.id_skladiste = '{1}'", broj_kalkulacije, skladiste);
            }

            string sql_stavke = string.Format(@"SELECT
                ks.sifra, ks.id_stavka AS id, r.naziv,           
                r.jm AS jmj, ks.kolicina, ks.rabat,
                ks.fak_cijena, ks.prijevoz, ks.carina,
                ks.posebni_porez, ks.marza_postotak as marza, ks.vpc,
                ks.porez AS pdv,
                pn.iznos as povratna_naknada
                FROM kalkulacija_stavke ks
                LEFT JOIN roba r ON cast(ks.sifra as text) = r.sifra
                LEFT JOIN povratna_naknada pn on ks.sifra = pn.sifra
                WHERE {0}
                ORDER BY CAST(id_stavka AS INT) ASC;", filter1);

            if (classSQL.remoteConnectionString == "")
            {
                classSQL.CeAdatpter(sql_stavke).Fill(dSkalkulacija_stavke, "DTkalkDtavke");
            }
            else
            {
                classSQL.NpgAdatpter(sql_stavke).Fill(dSkalkulacija_stavke, "DTkalkDtavke");
            }

            DataTable DT = dSkalkulacija_stavke.Tables[0];
            double fakIznos = 0;
            double fak_iznos_stavka = 0;
            double fakNetto = 0;
            double osnovica = 0;
            double fakPdv = 0;
            double fak_ukupno_stavka = 0;
            double fakUk = 0;
            double fak_cijena = 0;
            double kolicina = 0;
            double rabat = 0;
            double rabatStavka = 0;
            double rabatUk = 0;
            double mpc = 0;
            double mpc_uk = 0;
            double porez = 0;
            double vpc = 0;
            double nab_cijena = 0;
            double posebni_porez = 0;
            double prijevoz = 0;
            double carina = 0;
            double net_cijena = 0;
            double marza = 0;
            double marzaIznos = 0;
            double marzaUk = 0;
            double nabavna_zadnja = 0;
            double marza_uk_ = 0;
            double fak_cijena_S_pdv = 0;
            double fak_cijena_S_pdv_ukp = 0;
            double pov_nak = 0;
            double prijevozSaPdv = 0;
            decimal trosakBezPdv = 0;
            decimal trosakPdv = 0;
            decimal trosakUkupno = 0;

            // Dohvača podatke ako je firma u sustavu PDV-a
            DataTable DTpostavke = classSQL.select_settings("SELECT sustav_pdv FROM postavke", "postavke").Tables[0];
            int prikaziPdv = 0;

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                kolicina = Math.Round(Convert.ToDouble(DT.Rows[i]["kolicina"].ToString()), 3);
                fak_cijena = Math.Round(Convert.ToDouble(DT.Rows[i]["fak_cijena"].ToString()), 3);
                rabat = Math.Round(Convert.ToDouble(DT.Rows[i]["rabat"].ToString()), 2);
                porez = Math.Round(Convert.ToDouble(DT.Rows[i]["pdv"].ToString()), 3);
                vpc = Math.Round(Convert.ToDouble(DT.Rows[i]["vpc"].ToString()), 3);
                posebni_porez = Math.Round(Convert.ToDouble(DT.Rows[i]["posebni_porez"].ToString()), 3);
                prijevoz = Math.Round(Convert.ToDouble(DT.Rows[i]["prijevoz"].ToString()), 4);
                carina = Math.Round(Convert.ToDouble(DT.Rows[i]["carina"].ToString()), 3);
                marza = Math.Round(Convert.ToDouble(DT.Rows[i]["marza"].ToString()), 2);
                pov_nak = Math.Round(Convert.ToDouble(DT.Rows[i]["povratna_naknada"].ToString()), 3);

                if (!Class.Postavke.koristi_povratnu_naknadu)
                    pov_nak = 0;

                if (marza.ToString() == "NaN" || marza.ToString() == "Infinity")
                    marza = 0;
                double vpc_s_kol = vpc * kolicina;
                double fak_s_kol = fak_cijena * kolicina;
                nabavna_zadnja = Math.Round((fak_s_kol - (fak_s_kol * (rabat / 100)) + ((prijevoz + carina) * kolicina)), 3);
                marza_uk_ = Math.Round(nabavna_zadnja * (marza / 100), 3);
                marzaIznos = Math.Round(marza / 100 * nabavna_zadnja, 3);
                marzaUk += marzaIznos;

                rabatStavka = Math.Round((fak_cijena * (rabat / 100)), 2) * kolicina;
                rabatUk += rabatStavka;

                mpc = vpc * (1 + porez / 100.0) + pov_nak;

                mpc_uk = Math.Round(mpc, 2) * kolicina;

                prijevozSaPdv = ((prijevoz + carina) * kolicina) * 1.25;
                nab_cijena = Math.Round(fak_cijena * (1 - rabat / 100), 2, MidpointRounding.AwayFromZero) * kolicina;

                fak_cijena_S_pdv = nab_cijena * (1 + (porez / 100));
                if ((partnerExist && !uSustavuPdv) || DTpostavke.Rows[0]["sustav_pdv"].ToString() == "0")
                    fak_cijena_S_pdv = nab_cijena;

                fak_cijena_S_pdv_ukp += fak_cijena_S_pdv;

                net_cijena = kolicina * fak_cijena * (1 - rabat / 100);

                fak_iznos_stavka = kolicina * fak_cijena;
                fakIznos += fak_iznos_stavka;

                fakNetto += Math.Round((fak_cijena * (1 - rabat / 100)), 2, MidpointRounding.AwayFromZero) * kolicina;

                fak_ukupno_stavka = Math.Round(vpc * (1 + porez / 100), 2) * kolicina;
                fakUk += fak_ukupno_stavka;

                osnovica += fak_ukupno_stavka * 100 / (100 + porez);
                fakPdv += fak_ukupno_stavka * (1 - 100 / (100 + porez));

                // Stope
                double pdvStavka_cisto = nab_cijena * (porez / 100);
                int firmaSustavPdv = Convert.ToInt32(DTpostavke.Rows[0]["sustav_pdv"].ToString());
                if (firmaSustavPdv == 0)
                {
                    StopePDVa(Math.Round((decimal)porez, 4), 0, Math.Round(((decimal)nab_cijena), 4), true, false);
                    StopePDVa(Math.Round((decimal)porez, 4), 0, Math.Round(((decimal)(vpc * kolicina)), 4), false, false);
                }
                else
                {
                    if (partnerExist && !uSustavuPdv)
                    {
                        StopePDVa(Math.Round((decimal)porez, 4), 0, Math.Round(((decimal)nab_cijena), 4), true, false);
                        StopePDVa(Math.Round((decimal)porez, 4), Math.Round((decimal)(vpc * (porez / 100) * kolicina), 4), Math.Round(((decimal)(vpc * kolicina)), 4), false);
                    }
                    else if (partnerExist && uSustavuPdv)
                    {
                        StopePDVa(Math.Round((decimal)porez, 4), Math.Round((decimal)pdvStavka_cisto, 4), Math.Round(((decimal)nab_cijena), 4), true);
                        StopePDVa(Math.Round((decimal)porez, 4), Math.Round((decimal)(vpc * (porez / 100) * kolicina), 4), Math.Round(((decimal)(vpc * kolicina)), 4), false);
                        prikaziPdv = 1;
                    }
                }

                DT.Rows[i].SetField("mpc_uk", Math.Round(mpc_uk, 2, MidpointRounding.ToEven));
                DT.Rows[i].SetField("mpc", Math.Round(mpc, 3));
                DT.Rows[i].SetField("rabat", Math.Round(rabat, 2));
                DT.Rows[i].SetField("rabat_uk", Math.Round(rabatStavka, 2));
                DT.Rows[i].SetField("marza", Math.Round(marza, 3));
                DT.Rows[i].SetField("marzaIznos", Math.Round(marza_uk_, 3));
                DT.Rows[i].SetField("posebni_porez", posebni_porez);
                DT.Rows[i].SetField("prijevoz", Math.Round(prijevoz, 4));
                DT.Rows[i].SetField("carina", Math.Round(carina, 3));

                DT.Rows[i].SetField("kolicina", Math.Round(kolicina, 3));
                DT.Rows[i].SetField("fak_cijena", Math.Round(fak_cijena, 2));
                DT.Rows[i].SetField("fak_cijena_val", Math.Round(fak_cijena / tecaj, 3));
                DT.Rows[i].SetField("net_cijena", Math.Round(net_cijena, 3));
                DT.Rows[i].SetField("nab_cijena", Math.Round(nab_cijena, 3));
                DT.Rows[i].SetField("fak_cijena_S_pdv", Math.Round(fak_cijena_S_pdv, 2));

                DT.Rows[i].SetField("trosakBezPdv", Math.Round(trosakBezPdv, 3));
                DT.Rows[i].SetField("trosakPdv", Math.Round(trosakPdv, 3));
                DT.Rows[i].SetField("trosakUkupno", Math.Round(trosakUkupno, 3));
            }

            fakIznos = Math.Round(fakIznos, 2, MidpointRounding.AwayFromZero);
            fakNetto = Math.Round(fakNetto, 2, MidpointRounding.AwayFromZero);
            fakPdv = Math.Round(fakPdv, 3);
            osnovica = Math.Round(osnovica, 3);
            fakUk = Math.Round(fakUk, 2);
            rabatUk = Math.Round(rabatUk, 2);
            marzaUk = Math.Round(marzaUk, 3);
            string filter2 = "";
            if (pregled == true)
            {
                filter2 = "kalkulacija.id_kalkulacija='" + broj_kalkulacije + "';";
            }
            else
            {
                filter2 = "kalkulacija.broj='" + broj_kalkulacije + "' AND kalkulacija.id_skladiste='" + skladiste + "';";
            }

            // Trošak kalkulacija
            if (DTpostavke.Rows[0]["sustav_pdv"].ToString() == "1")
            {
                if (uSustavuPdv)
                    TrosakSaPdv(trosak, out trosakBezPdv, out trosakPdv, out trosakUkupno);
                else
                    TrosakBezPdv(trosak, out trosakBezPdv, out trosakPdv, out trosakUkupno);
            }
            else
                TrosakBezPdv(trosak, out trosakBezPdv, out trosakPdv, out trosakUkupno);

            if (DTpostavke.Rows[0]["sustav_pdv"].ToString() == "0" || !uSustavuPdv)
                fak_cijena_S_pdv_ukp = fakNetto + (double)trosakUkupno;

            CheckStope((decimal)fakNetto, trosakBezPdv, trosakPdv);

            string sql_kalk = $@"SELECT kalkulacija.broj, kalkulacija.id_partner, kalkulacija.godina, 'Kalkulacija' AS naslov, skladiste.skladiste, kalkulacija.datum,
partners.ime_tvrtke + ' ' + partners.id_partner AS dobavljac, kalkulacija.tecaj AS tecaj, kalkulacija.racun, kalkulacija.otpremnica,
CAST(kalkulacija.otpremnica_datum AS date) AS otpremnica_datum, kalkulacija.tecaj, zaposlenici.ime + ' ' + zaposlenici.prezime AS kalkulirao,
{trosakBezPdv.ToString().Replace(',', '.')} AS trosakBezPdv, {trosakPdv.ToString().Replace(',', '.')} AS trosakPdv,  {trosakUkupno.ToString().Replace(',', '.')} AS trosakUkupno, {prikaziPdv} AS prikaziPdv, {nbcPdvUkupno.ToString().Replace(',', '.')} AS nbcPdvUkupno,
CAST(kalkulacija.racun_datum AS date) AS racun_datum, valute.ime_valute AS valuta, '" + fakIznos + @"' AS fak_iznos, '" + fakNetto + @"' AS netto_fak_iznos,
'" + fakPdv + @"' AS pdv, '" + rabatUk + @"' AS rabatUk, '" + marzaUk + @"' AS marzaUk, '" + osnovica + @"' AS osnovica, '" + fakUk + @"' AS ukupno,
'" + Math.Round(fak_cijena_S_pdv_ukp, 2) + @"' as fak_cijena_S_pdv, case when partners.uSustavPdv = true then 'Dobavljač u sustavu PDV-a: DA' else 'Dobavljač u sustavu PDV-a: NE' end as sustavPdv
FROM kalkulacija
LEFT JOIN skladiste ON kalkulacija.id_skladiste=skladiste.id_skladiste
LEFT JOIN zaposlenici ON kalkulacija.id_zaposlenik=zaposlenici.id_zaposlenik
LEFT JOIN valute ON kalkulacija.id_valuta=valute.id_valuta
LEFT JOIN partners ON kalkulacija.id_partner=partners.id_partner
WHERE " + filter2;

            if (classSQL.remoteConnectionString == "")
            {
                classSQL.CeAdatpter(sql_kalk).Fill(dSkalkulacija, "DTKalkulacijA");
            }
            else
            {
                classSQL.NpgAdatpter(sql_kalk).Fill(dSkalkulacija, "DTKalkulacijA");
            }
        }

        private void StopePDVa(decimal pdv, decimal iznos, decimal osnovica, bool nbc = true, bool showStopa = true)
        {
            DataRow[] dataROW = dSstope.Tables["DTstope"].Select("stopa = '" + Math.Round(pdv).ToString() + (nbc ? "¤nbc" : "¤mpc") + "'");

            if (dataROW.Count() == 0)
            {
                RowPdv = dSstope.Tables["DTstope"].NewRow();
                if (showStopa)
                    RowPdv["stopa"] = Math.Round(pdv, 0) + (nbc ? "¤nbc" : "¤mpc");
                else
                    RowPdv["stopa"] = 0 + (nbc ? "¤nbc" : "¤mpc");
                RowPdv["iznos"] = Math.Round(iznos, 2, MidpointRounding.AwayFromZero);
                RowPdv["osnovica"] = Math.Round(osnovica, 2, MidpointRounding.AwayFromZero);
                RowPdv["vrsta"] = "roba";
                dSstope.Tables["DTstope"].Rows.Add(RowPdv);
            }
            else
            {
                dataROW[0]["iznos"] = Convert.ToDecimal(dataROW[0]["iznos"].ToString()) + Math.Round(iznos, 3);
                dataROW[0]["osnovica"] = Convert.ToDecimal(dataROW[0]["osnovica"].ToString()) + Math.Round(osnovica, 3);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="osnovica"></param>
        private void CheckStope(decimal osnovica, decimal trosakBezPdv = 0, decimal trosakPdv = 0)
        {
            DataTable dataTable = dSstope.Tables["DTstope"];
            int count = dataTable.Rows.Count;
            List<DataRow> rowList = new List<DataRow>();
            int zeroAmount = 0;

            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (dataTable.Rows[i][0].ToString().Contains("nbc"))
                    {
                        string[] split = dataTable.Rows[i][0].ToString().Split('¤');
                        decimal stopa = Convert.ToDecimal(split[0].ToString());
                        if (stopa > 0 && dataTable.Rows[i]["vrsta"].ToString() == "roba")
                            nbcPdvUkupno += Convert.ToDecimal(dataTable.Rows[i]["iznos"].ToString());
                        dataTable.Rows[i]["trosak_pdv"] = trosakPdv;
                        dataTable.Rows[i]["trosak_ukupno"] = trosakBezPdv;
                        if (split[0] == "0")
                        {
                            zeroAmount++;
                            rowList.Add(dataTable.Rows[i]);
                        }
                    }
                    else
                    {
                        dataTable.Rows[i]["trosak_pdv"] = 0;
                        dataTable.Rows[i]["trosak_ukupno"] = trosakBezPdv;
                    }
                }


                if (zeroAmount > 1)
                {
                    foreach (DataRow item in rowList)
                    {
                        dSstope.Tables["DTstope"].Rows.Remove(item);
                    }

                    DataRow row = dSstope.Tables["DTstope"].NewRow();
                    row["stopa"] = 0 + "¤nbc";
                    row["osnovica"] = Math.Round(osnovica + trosakBezPdv, 3);
                    row["iznos"] = 0;
                    dSstope.Tables["DTstope"].Rows.Add(row);
                }
            }
        }

        /// <summary>
        /// Used to calculate total "trosak" without taxes
        /// </summary>
        /// <param name="trosak"></param>
        /// <param name="porez"></param>
        /// <param name="trosakBezPdv"></param>
        /// <param name="trosakUkupno"></param>
        private void TrosakSaPdv(decimal trosak, out decimal trosakBezPdv, out decimal trosakPdv, out decimal trosakUkupno)
        {
            trosakBezPdv = trosak;
            trosakPdv = trosak * 0.25m;
            trosakUkupno = trosakBezPdv + trosakPdv;
            AddToStope(25, trosakBezPdv, trosakPdv, "nbc");
        }

        /// <summary>
        /// Used to calculate total "trosak" with taxes
        /// </summary>
        /// <param name="trosak"></param>
        /// <param name="porez"></param>
        /// <param name="trosakBezPdv"></param>
        /// <param name="trosakPdv"></param>
        /// <param name="trosakUkupno"></param>
        private void TrosakBezPdv(decimal trosak, out decimal trosakBezPdv, out decimal trosakPdv, out decimal trosakUkupno)
        {
            trosakBezPdv = trosak;
            trosakPdv = 0;
            trosakUkupno = trosakBezPdv;
            AddToStope(0, trosakBezPdv, trosakPdv, "nbc");
        }

        /// <summary>
        /// Adds new row to dSstope, table DTstope
        /// </summary>
        /// <param name="stopa"></param>
        /// <param name="osnovica"></param>
        /// <param name="iznos"></param>
        /// <param name="type"></param>
        private void AddToStope(int stopa, decimal osnovica, decimal iznos, string type)
        {
            DataRow row = dSstope.Tables["DTstope"].NewRow();
            row["stopa"] = stopa + $"¤{type}";
            row["osnovica"] = Math.Round(osnovica, 3);
            row["iznos"] = iznos;
            row["vrsta"] = "trosak";
            dSstope.Tables["DTstope"].Rows.Add(row);
        }
    }
}