namespace KS2
{
    partial class ZakresNutDialog
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
            this.wiolinowy_od = new System.Windows.Forms.ComboBox();
            this.wiolinowy_do = new System.Windows.Forms.ComboBox();
            this.OK = new System.Windows.Forms.Button();
            this.anuluj = new System.Windows.Forms.Button();
            this.zakres_wiolinowy_groupBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.wiolinowy_alert_label = new System.Windows.Forms.Label();
            this.klucz_label = new System.Windows.Forms.Label();
            this.klucz_combobox = new System.Windows.Forms.ComboBox();
            this.zakres_basowy_groupBox = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.basowy_od = new System.Windows.Forms.ComboBox();
            this.basowy_do = new System.Windows.Forms.ComboBox();
            this.basowy_alert_label = new System.Windows.Forms.Label();
            this.znaki_chromatycze_groupBox = new System.Windows.Forms.GroupBox();
            this.kasownik_checkBox = new System.Windows.Forms.CheckBox();
            this.podwojny_krzyzyk_checkBox = new System.Windows.Forms.CheckBox();
            this.podwojny_bemol_checkBox = new System.Windows.Forms.CheckBox();
            this.znaki_chromatyczne_alert_label = new System.Windows.Forms.Label();
            this.brak_checkBox = new System.Windows.Forms.CheckBox();
            this.krzyzyk_checkBox = new System.Windows.Forms.CheckBox();
            this.bemol_checkBox = new System.Windows.Forms.CheckBox();
            this.wartosci_rytmiczne_groupBox = new System.Windows.Forms.GroupBox();
            this.wartosci_rytmiczne_alert_label = new System.Windows.Forms.Label();
            this.szesnastka_checkBox = new System.Windows.Forms.CheckBox();
            this.osemka_checkBox = new System.Windows.Forms.CheckBox();
            this.polnuta_checkBox = new System.Windows.Forms.CheckBox();
            this.cwiercnuta_checkBox = new System.Windows.Forms.CheckBox();
            this.cala_nuta_checkBox = new System.Windows.Forms.CheckBox();
            this.pauzy_groupBox = new System.Windows.Forms.GroupBox();
            this.pauza_szesnastkowa_checkBox = new System.Windows.Forms.CheckBox();
            this.pauza_osemkowa_checkBox = new System.Windows.Forms.CheckBox();
            this.pauza_polnutowa_checkBox = new System.Windows.Forms.CheckBox();
            this.pauza_cwiercnutowa_checkBox = new System.Windows.Forms.CheckBox();
            this.pauza_calonutowa_checkBox = new System.Windows.Forms.CheckBox();
            this.symbole_ustawione_alert_label = new System.Windows.Forms.Label();
            this.klucz_groupBox = new System.Windows.Forms.GroupBox();
            this.zakres_wiolinowy_groupBox.SuspendLayout();
            this.zakres_basowy_groupBox.SuspendLayout();
            this.znaki_chromatycze_groupBox.SuspendLayout();
            this.wartosci_rytmiczne_groupBox.SuspendLayout();
            this.pauzy_groupBox.SuspendLayout();
            this.klucz_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // wiolinowy_od
            // 
            this.wiolinowy_od.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.wiolinowy_od.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.wiolinowy_od.FormattingEnabled = true;
            this.wiolinowy_od.Location = new System.Drawing.Point(87, 23);
            this.wiolinowy_od.Name = "wiolinowy_od";
            this.wiolinowy_od.Size = new System.Drawing.Size(177, 22);
            this.wiolinowy_od.TabIndex = 0;
            this.wiolinowy_od.SelectedIndexChanged += new System.EventHandler(this.zakres_nut_dolny_SelectedIndexChanged);
            // 
            // wiolinowy_do
            // 
            this.wiolinowy_do.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.wiolinowy_do.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.wiolinowy_do.FormattingEnabled = true;
            this.wiolinowy_do.Location = new System.Drawing.Point(312, 23);
            this.wiolinowy_do.Name = "wiolinowy_do";
            this.wiolinowy_do.Size = new System.Drawing.Size(186, 22);
            this.wiolinowy_do.TabIndex = 1;
            this.wiolinowy_do.SelectedIndexChanged += new System.EventHandler(this.wiolinowy_do_SelectedIndexChanged);
            // 
            // OK
            // 
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(792, 471);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 2;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // anuluj
            // 
            this.anuluj.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.anuluj.Location = new System.Drawing.Point(711, 471);
            this.anuluj.Name = "anuluj";
            this.anuluj.Size = new System.Drawing.Size(75, 23);
            this.anuluj.TabIndex = 3;
            this.anuluj.Text = "Anuluj";
            this.anuluj.UseVisualStyleBackColor = true;
            this.anuluj.Click += new System.EventHandler(this.anuluj_Click);
            // 
            // zakres_wiolinowy_groupBox
            // 
            this.zakres_wiolinowy_groupBox.Controls.Add(this.label2);
            this.zakres_wiolinowy_groupBox.Controls.Add(this.label1);
            this.zakres_wiolinowy_groupBox.Controls.Add(this.wiolinowy_od);
            this.zakres_wiolinowy_groupBox.Controls.Add(this.wiolinowy_do);
            this.zakres_wiolinowy_groupBox.Controls.Add(this.wiolinowy_alert_label);
            this.zakres_wiolinowy_groupBox.Location = new System.Drawing.Point(17, 71);
            this.zakres_wiolinowy_groupBox.Name = "zakres_wiolinowy_groupBox";
            this.zakres_wiolinowy_groupBox.Size = new System.Drawing.Size(818, 52);
            this.zakres_wiolinowy_groupBox.TabIndex = 4;
            this.zakres_wiolinowy_groupBox.TabStop = false;
            this.zakres_wiolinowy_groupBox.Text = "Zakres klucza wiolinowego";
            this.zakres_wiolinowy_groupBox.Enter += new System.EventHandler(this.zakres_wiolinowy_groupBox_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(287, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "do";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "dźwięki od";
            // 
            // wiolinowy_alert_label
            // 
            this.wiolinowy_alert_label.AutoSize = true;
            this.wiolinowy_alert_label.BackColor = System.Drawing.SystemColors.Control;
            this.wiolinowy_alert_label.ForeColor = System.Drawing.Color.Red;
            this.wiolinowy_alert_label.Location = new System.Drawing.Point(529, 26);
            this.wiolinowy_alert_label.Name = "wiolinowy_alert_label";
            this.wiolinowy_alert_label.Size = new System.Drawing.Size(35, 13);
            this.wiolinowy_alert_label.TabIndex = 7;
            this.wiolinowy_alert_label.Text = "label7";
            // 
            // klucz_label
            // 
            this.klucz_label.AutoSize = true;
            this.klucz_label.Location = new System.Drawing.Point(24, 33);
            this.klucz_label.Name = "klucz_label";
            this.klucz_label.Size = new System.Drawing.Size(74, 13);
            this.klucz_label.TabIndex = 6;
            this.klucz_label.Text = "Nazwa klucza";
            this.klucz_label.Click += new System.EventHandler(this.klucz_label_Click);
            // 
            // klucz_combobox
            // 
            this.klucz_combobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.klucz_combobox.FormattingEnabled = true;
            this.klucz_combobox.Location = new System.Drawing.Point(105, 33);
            this.klucz_combobox.Name = "klucz_combobox";
            this.klucz_combobox.Size = new System.Drawing.Size(176, 21);
            this.klucz_combobox.TabIndex = 6;
            this.klucz_combobox.SelectedIndexChanged += new System.EventHandler(this.klucz_SelectedIndexChanged);
            // 
            // zakres_basowy_groupBox
            // 
            this.zakres_basowy_groupBox.Controls.Add(this.label5);
            this.zakres_basowy_groupBox.Controls.Add(this.label6);
            this.zakres_basowy_groupBox.Controls.Add(this.basowy_od);
            this.zakres_basowy_groupBox.Controls.Add(this.basowy_do);
            this.zakres_basowy_groupBox.Controls.Add(this.basowy_alert_label);
            this.zakres_basowy_groupBox.Location = new System.Drawing.Point(19, 129);
            this.zakres_basowy_groupBox.Name = "zakres_basowy_groupBox";
            this.zakres_basowy_groupBox.Size = new System.Drawing.Size(814, 53);
            this.zakres_basowy_groupBox.TabIndex = 5;
            this.zakres_basowy_groupBox.TabStop = false;
            this.zakres_basowy_groupBox.Text = "Zakres klucza basowego";
            this.zakres_basowy_groupBox.Enter += new System.EventHandler(this.zakres_basowy_groupBox_Enter);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(286, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "do";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "dźwięki od";
            // 
            // basowy_od
            // 
            this.basowy_od.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.basowy_od.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.basowy_od.FormattingEnabled = true;
            this.basowy_od.Location = new System.Drawing.Point(86, 23);
            this.basowy_od.Name = "basowy_od";
            this.basowy_od.Size = new System.Drawing.Size(177, 22);
            this.basowy_od.TabIndex = 0;
            this.basowy_od.SelectedIndexChanged += new System.EventHandler(this.basowy_od_SelectedIndexChanged);
            // 
            // basowy_do
            // 
            this.basowy_do.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.basowy_do.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.basowy_do.FormattingEnabled = true;
            this.basowy_do.Location = new System.Drawing.Point(311, 23);
            this.basowy_do.Name = "basowy_do";
            this.basowy_do.Size = new System.Drawing.Size(186, 22);
            this.basowy_do.TabIndex = 1;
            this.basowy_do.SelectedIndexChanged += new System.EventHandler(this.basowy_do_SelectedIndexChanged);
            // 
            // basowy_alert_label
            // 
            this.basowy_alert_label.AutoSize = true;
            this.basowy_alert_label.BackColor = System.Drawing.SystemColors.Control;
            this.basowy_alert_label.ForeColor = System.Drawing.Color.Red;
            this.basowy_alert_label.Location = new System.Drawing.Point(528, 26);
            this.basowy_alert_label.Name = "basowy_alert_label";
            this.basowy_alert_label.Size = new System.Drawing.Size(35, 13);
            this.basowy_alert_label.TabIndex = 8;
            this.basowy_alert_label.Text = "label7";
            // 
            // znaki_chromatycze_groupBox
            // 
            this.znaki_chromatycze_groupBox.Controls.Add(this.kasownik_checkBox);
            this.znaki_chromatycze_groupBox.Controls.Add(this.podwojny_krzyzyk_checkBox);
            this.znaki_chromatycze_groupBox.Controls.Add(this.podwojny_bemol_checkBox);
            this.znaki_chromatycze_groupBox.Controls.Add(this.znaki_chromatyczne_alert_label);
            this.znaki_chromatycze_groupBox.Controls.Add(this.brak_checkBox);
            this.znaki_chromatycze_groupBox.Controls.Add(this.krzyzyk_checkBox);
            this.znaki_chromatycze_groupBox.Controls.Add(this.bemol_checkBox);
            this.znaki_chromatycze_groupBox.Location = new System.Drawing.Point(19, 216);
            this.znaki_chromatycze_groupBox.Name = "znaki_chromatycze_groupBox";
            this.znaki_chromatycze_groupBox.Size = new System.Drawing.Size(836, 70);
            this.znaki_chromatycze_groupBox.TabIndex = 9;
            this.znaki_chromatycze_groupBox.TabStop = false;
            this.znaki_chromatycze_groupBox.Text = "Znaki chromatyczne";
            // 
            // kasownik_checkBox
            // 
            this.kasownik_checkBox.AutoSize = true;
            this.kasownik_checkBox.Location = new System.Drawing.Point(533, 19);
            this.kasownik_checkBox.Name = "kasownik_checkBox";
            this.kasownik_checkBox.Size = new System.Drawing.Size(71, 17);
            this.kasownik_checkBox.TabIndex = 16;
            this.kasownik_checkBox.Text = "kasownik";
            this.kasownik_checkBox.UseVisualStyleBackColor = true;
            this.kasownik_checkBox.CheckedChanged += new System.EventHandler(this.checkBox10_CheckedChanged);
            // 
            // podwojny_krzyzyk_checkBox
            // 
            this.podwojny_krzyzyk_checkBox.AutoSize = true;
            this.podwojny_krzyzyk_checkBox.Location = new System.Drawing.Point(406, 20);
            this.podwojny_krzyzyk_checkBox.Name = "podwojny_krzyzyk_checkBox";
            this.podwojny_krzyzyk_checkBox.Size = new System.Drawing.Size(93, 17);
            this.podwojny_krzyzyk_checkBox.TabIndex = 4;
            this.podwojny_krzyzyk_checkBox.Text = "podw. krzyżyk";
            this.podwojny_krzyzyk_checkBox.UseVisualStyleBackColor = true;
            this.podwojny_krzyzyk_checkBox.CheckedChanged += new System.EventHandler(this.pod_krzyzyk_checkBox_CheckedChanged);
            // 
            // podwojny_bemol_checkBox
            // 
            this.podwojny_bemol_checkBox.AutoSize = true;
            this.podwojny_bemol_checkBox.Location = new System.Drawing.Point(288, 19);
            this.podwojny_bemol_checkBox.Name = "podwojny_bemol_checkBox";
            this.podwojny_bemol_checkBox.Size = new System.Drawing.Size(86, 17);
            this.podwojny_bemol_checkBox.TabIndex = 3;
            this.podwojny_bemol_checkBox.Text = "podw. bemol";
            this.podwojny_bemol_checkBox.UseVisualStyleBackColor = true;
            this.podwojny_bemol_checkBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // znaki_chromatyczne_alert_label
            // 
            this.znaki_chromatyczne_alert_label.AutoSize = true;
            this.znaki_chromatyczne_alert_label.BackColor = System.Drawing.SystemColors.Control;
            this.znaki_chromatyczne_alert_label.ForeColor = System.Drawing.Color.Red;
            this.znaki_chromatyczne_alert_label.Location = new System.Drawing.Point(12, 43);
            this.znaki_chromatyczne_alert_label.Name = "znaki_chromatyczne_alert_label";
            this.znaki_chromatyczne_alert_label.Size = new System.Drawing.Size(35, 13);
            this.znaki_chromatyczne_alert_label.TabIndex = 10;
            this.znaki_chromatyczne_alert_label.Text = "label7";
            this.znaki_chromatyczne_alert_label.Click += new System.EventHandler(this.znaki_chromatyczne_alert_label_Click);
            // 
            // brak_checkBox
            // 
            this.brak_checkBox.AutoSize = true;
            this.brak_checkBox.Location = new System.Drawing.Point(15, 19);
            this.brak_checkBox.Name = "brak_checkBox";
            this.brak_checkBox.Size = new System.Drawing.Size(47, 17);
            this.brak_checkBox.TabIndex = 2;
            this.brak_checkBox.Text = "brak";
            this.brak_checkBox.UseVisualStyleBackColor = true;
            this.brak_checkBox.CheckedChanged += new System.EventHandler(this.znaki_brak_checkBox_CheckedChanged);
            // 
            // krzyzyk_checkBox
            // 
            this.krzyzyk_checkBox.AutoSize = true;
            this.krzyzyk_checkBox.Location = new System.Drawing.Point(193, 20);
            this.krzyzyk_checkBox.Name = "krzyzyk_checkBox";
            this.krzyzyk_checkBox.Size = new System.Drawing.Size(61, 17);
            this.krzyzyk_checkBox.TabIndex = 1;
            this.krzyzyk_checkBox.Text = "krzyżyk";
            this.krzyzyk_checkBox.UseVisualStyleBackColor = true;
            this.krzyzyk_checkBox.CheckedChanged += new System.EventHandler(this.krzyzyk_checkBox_CheckedChanged);
            // 
            // bemol_checkBox
            // 
            this.bemol_checkBox.AutoSize = true;
            this.bemol_checkBox.Location = new System.Drawing.Point(101, 20);
            this.bemol_checkBox.Name = "bemol_checkBox";
            this.bemol_checkBox.Size = new System.Drawing.Size(54, 17);
            this.bemol_checkBox.TabIndex = 0;
            this.bemol_checkBox.Text = "bemol";
            this.bemol_checkBox.UseVisualStyleBackColor = true;
            this.bemol_checkBox.CheckedChanged += new System.EventHandler(this.bemol_checkBox_CheckedChanged);
            // 
            // wartosci_rytmiczne_groupBox
            // 
            this.wartosci_rytmiczne_groupBox.Controls.Add(this.wartosci_rytmiczne_alert_label);
            this.wartosci_rytmiczne_groupBox.Controls.Add(this.szesnastka_checkBox);
            this.wartosci_rytmiczne_groupBox.Controls.Add(this.osemka_checkBox);
            this.wartosci_rytmiczne_groupBox.Controls.Add(this.polnuta_checkBox);
            this.wartosci_rytmiczne_groupBox.Controls.Add(this.cwiercnuta_checkBox);
            this.wartosci_rytmiczne_groupBox.Controls.Add(this.cala_nuta_checkBox);
            this.wartosci_rytmiczne_groupBox.Location = new System.Drawing.Point(20, 292);
            this.wartosci_rytmiczne_groupBox.Name = "wartosci_rytmiczne_groupBox";
            this.wartosci_rytmiczne_groupBox.Size = new System.Drawing.Size(839, 81);
            this.wartosci_rytmiczne_groupBox.TabIndex = 11;
            this.wartosci_rytmiczne_groupBox.TabStop = false;
            this.wartosci_rytmiczne_groupBox.Text = "Wartości rytmiczne";
            this.wartosci_rytmiczne_groupBox.Enter += new System.EventHandler(this.wartosci_rytmiczne_groupBox_Enter);
            // 
            // wartosci_rytmiczne_alert_label
            // 
            this.wartosci_rytmiczne_alert_label.AutoSize = true;
            this.wartosci_rytmiczne_alert_label.BackColor = System.Drawing.SystemColors.Control;
            this.wartosci_rytmiczne_alert_label.ForeColor = System.Drawing.Color.Red;
            this.wartosci_rytmiczne_alert_label.Location = new System.Drawing.Point(11, 52);
            this.wartosci_rytmiczne_alert_label.Name = "wartosci_rytmiczne_alert_label";
            this.wartosci_rytmiczne_alert_label.Size = new System.Drawing.Size(35, 13);
            this.wartosci_rytmiczne_alert_label.TabIndex = 12;
            this.wartosci_rytmiczne_alert_label.Text = "label7";
            // 
            // szesnastka_checkBox
            // 
            this.szesnastka_checkBox.AutoSize = true;
            this.szesnastka_checkBox.Location = new System.Drawing.Point(405, 32);
            this.szesnastka_checkBox.Name = "szesnastka_checkBox";
            this.szesnastka_checkBox.Size = new System.Drawing.Size(79, 17);
            this.szesnastka_checkBox.TabIndex = 5;
            this.szesnastka_checkBox.Text = "szesnastka";
            this.szesnastka_checkBox.UseVisualStyleBackColor = true;
            this.szesnastka_checkBox.CheckedChanged += new System.EventHandler(this.szesnastka_checkBox_CheckedChanged);
            // 
            // osemka_checkBox
            // 
            this.osemka_checkBox.AutoSize = true;
            this.osemka_checkBox.Location = new System.Drawing.Point(310, 32);
            this.osemka_checkBox.Name = "osemka_checkBox";
            this.osemka_checkBox.Size = new System.Drawing.Size(63, 17);
            this.osemka_checkBox.TabIndex = 4;
            this.osemka_checkBox.Text = "ósemka";
            this.osemka_checkBox.UseVisualStyleBackColor = true;
            this.osemka_checkBox.CheckedChanged += new System.EventHandler(this.osemka_checkBox_CheckedChanged);
            // 
            // polnuta_checkBox
            // 
            this.polnuta_checkBox.AutoSize = true;
            this.polnuta_checkBox.Location = new System.Drawing.Point(111, 32);
            this.polnuta_checkBox.Name = "polnuta_checkBox";
            this.polnuta_checkBox.Size = new System.Drawing.Size(61, 17);
            this.polnuta_checkBox.TabIndex = 3;
            this.polnuta_checkBox.Text = "pólnuta";
            this.polnuta_checkBox.UseVisualStyleBackColor = true;
            this.polnuta_checkBox.CheckedChanged += new System.EventHandler(this.polnuta_checkBox_CheckedChanged);
            // 
            // cwiercnuta_checkBox
            // 
            this.cwiercnuta_checkBox.AutoSize = true;
            this.cwiercnuta_checkBox.Location = new System.Drawing.Point(203, 32);
            this.cwiercnuta_checkBox.Name = "cwiercnuta_checkBox";
            this.cwiercnuta_checkBox.Size = new System.Drawing.Size(78, 17);
            this.cwiercnuta_checkBox.TabIndex = 3;
            this.cwiercnuta_checkBox.Text = "ćwierćnuta";
            this.cwiercnuta_checkBox.UseVisualStyleBackColor = true;
            this.cwiercnuta_checkBox.CheckedChanged += new System.EventHandler(this.cwiercnuta_checkBox_CheckedChanged);
            // 
            // cala_nuta_checkBox
            // 
            this.cala_nuta_checkBox.AutoSize = true;
            this.cala_nuta_checkBox.Location = new System.Drawing.Point(14, 32);
            this.cala_nuta_checkBox.Name = "cala_nuta_checkBox";
            this.cala_nuta_checkBox.Size = new System.Drawing.Size(72, 17);
            this.cala_nuta_checkBox.TabIndex = 3;
            this.cala_nuta_checkBox.Text = "cała nuta";
            this.cala_nuta_checkBox.UseVisualStyleBackColor = true;
            this.cala_nuta_checkBox.CheckedChanged += new System.EventHandler(this.cala_nuta_checkBox_CheckedChanged);
            // 
            // pauzy_groupBox
            // 
            this.pauzy_groupBox.Controls.Add(this.pauza_szesnastkowa_checkBox);
            this.pauzy_groupBox.Controls.Add(this.pauza_osemkowa_checkBox);
            this.pauzy_groupBox.Controls.Add(this.pauza_polnutowa_checkBox);
            this.pauzy_groupBox.Controls.Add(this.pauza_cwiercnutowa_checkBox);
            this.pauzy_groupBox.Controls.Add(this.pauza_calonutowa_checkBox);
            this.pauzy_groupBox.Controls.Add(this.symbole_ustawione_alert_label);
            this.pauzy_groupBox.Location = new System.Drawing.Point(20, 379);
            this.pauzy_groupBox.Name = "pauzy_groupBox";
            this.pauzy_groupBox.Size = new System.Drawing.Size(836, 86);
            this.pauzy_groupBox.TabIndex = 12;
            this.pauzy_groupBox.TabStop = false;
            this.pauzy_groupBox.Text = "Pauzy";
            this.pauzy_groupBox.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // pauza_szesnastkowa_checkBox
            // 
            this.pauza_szesnastkowa_checkBox.AutoSize = true;
            this.pauza_szesnastkowa_checkBox.Location = new System.Drawing.Point(406, 21);
            this.pauza_szesnastkowa_checkBox.Name = "pauza_szesnastkowa_checkBox";
            this.pauza_szesnastkowa_checkBox.Size = new System.Drawing.Size(93, 17);
            this.pauza_szesnastkowa_checkBox.TabIndex = 15;
            this.pauza_szesnastkowa_checkBox.Text = "szesnastkowa";
            this.pauza_szesnastkowa_checkBox.UseVisualStyleBackColor = true;
            this.pauza_szesnastkowa_checkBox.CheckedChanged += new System.EventHandler(this.pauza_szesnastkowa_checkBox_CheckedChanged);
            // 
            // pauza_osemkowa_checkBox
            // 
            this.pauza_osemkowa_checkBox.AutoSize = true;
            this.pauza_osemkowa_checkBox.Location = new System.Drawing.Point(311, 20);
            this.pauza_osemkowa_checkBox.Name = "pauza_osemkowa_checkBox";
            this.pauza_osemkowa_checkBox.Size = new System.Drawing.Size(77, 17);
            this.pauza_osemkowa_checkBox.TabIndex = 14;
            this.pauza_osemkowa_checkBox.Text = "ósemkowa";
            this.pauza_osemkowa_checkBox.UseVisualStyleBackColor = true;
            this.pauza_osemkowa_checkBox.CheckedChanged += new System.EventHandler(this.pauza_osemkowa_checkBox_CheckedChanged);
            // 
            // pauza_polnutowa_checkBox
            // 
            this.pauza_polnutowa_checkBox.AutoSize = true;
            this.pauza_polnutowa_checkBox.Location = new System.Drawing.Point(112, 21);
            this.pauza_polnutowa_checkBox.Name = "pauza_polnutowa_checkBox";
            this.pauza_polnutowa_checkBox.Size = new System.Drawing.Size(75, 17);
            this.pauza_polnutowa_checkBox.TabIndex = 11;
            this.pauza_polnutowa_checkBox.Text = "pólnutowa";
            this.pauza_polnutowa_checkBox.UseVisualStyleBackColor = true;
            this.pauza_polnutowa_checkBox.CheckedChanged += new System.EventHandler(this.pauza_polnutowa_checkBox_CheckedChanged);
            // 
            // pauza_cwiercnutowa_checkBox
            // 
            this.pauza_cwiercnutowa_checkBox.AutoSize = true;
            this.pauza_cwiercnutowa_checkBox.Location = new System.Drawing.Point(204, 21);
            this.pauza_cwiercnutowa_checkBox.Name = "pauza_cwiercnutowa_checkBox";
            this.pauza_cwiercnutowa_checkBox.Size = new System.Drawing.Size(92, 17);
            this.pauza_cwiercnutowa_checkBox.TabIndex = 12;
            this.pauza_cwiercnutowa_checkBox.Text = "ćwierćnutowa";
            this.pauza_cwiercnutowa_checkBox.UseVisualStyleBackColor = true;
            this.pauza_cwiercnutowa_checkBox.CheckedChanged += new System.EventHandler(this.pauza_cwiercnutowa_checkBox_CheckedChanged);
            // 
            // pauza_calonutowa_checkBox
            // 
            this.pauza_calonutowa_checkBox.AutoSize = true;
            this.pauza_calonutowa_checkBox.Location = new System.Drawing.Point(15, 21);
            this.pauza_calonutowa_checkBox.Name = "pauza_calonutowa_checkBox";
            this.pauza_calonutowa_checkBox.Size = new System.Drawing.Size(83, 17);
            this.pauza_calonutowa_checkBox.TabIndex = 13;
            this.pauza_calonutowa_checkBox.Text = "całonutowa";
            this.pauza_calonutowa_checkBox.UseVisualStyleBackColor = true;
            this.pauza_calonutowa_checkBox.CheckedChanged += new System.EventHandler(this.pauza_calonutowa_checkBox_CheckedChanged);
            // 
            // symbole_ustawione_alert_label
            // 
            this.symbole_ustawione_alert_label.AutoSize = true;
            this.symbole_ustawione_alert_label.BackColor = System.Drawing.SystemColors.Control;
            this.symbole_ustawione_alert_label.ForeColor = System.Drawing.Color.Red;
            this.symbole_ustawione_alert_label.Location = new System.Drawing.Point(12, 55);
            this.symbole_ustawione_alert_label.Name = "symbole_ustawione_alert_label";
            this.symbole_ustawione_alert_label.Size = new System.Drawing.Size(35, 13);
            this.symbole_ustawione_alert_label.TabIndex = 10;
            this.symbole_ustawione_alert_label.Text = "label7";
            // 
            // klucz_groupBox
            // 
            this.klucz_groupBox.Controls.Add(this.klucz_label);
            this.klucz_groupBox.Controls.Add(this.klucz_combobox);
            this.klucz_groupBox.Controls.Add(this.zakres_wiolinowy_groupBox);
            this.klucz_groupBox.Controls.Add(this.zakres_basowy_groupBox);
            this.klucz_groupBox.Location = new System.Drawing.Point(20, 12);
            this.klucz_groupBox.Name = "klucz_groupBox";
            this.klucz_groupBox.Size = new System.Drawing.Size(839, 198);
            this.klucz_groupBox.TabIndex = 13;
            this.klucz_groupBox.TabStop = false;
            this.klucz_groupBox.Text = "Klucz";
            this.klucz_groupBox.Enter += new System.EventHandler(this.groupBox1_Enter_1);
            // 
            // ZakresNutDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 521);
            this.Controls.Add(this.klucz_groupBox);
            this.Controls.Add(this.pauzy_groupBox);
            this.Controls.Add(this.wartosci_rytmiczne_groupBox);
            this.Controls.Add(this.znaki_chromatycze_groupBox);
            this.Controls.Add(this.anuluj);
            this.Controls.Add(this.OK);
            this.KeyPreview = true;
            this.Name = "ZakresNutDialog";
            this.Text = "Zakres ćwiczeń";
            this.Load += new System.EventHandler(this.ZakresNutDialog_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ZakresNutDialog_KeyDown);
            this.zakres_wiolinowy_groupBox.ResumeLayout(false);
            this.zakres_wiolinowy_groupBox.PerformLayout();
            this.zakres_basowy_groupBox.ResumeLayout(false);
            this.zakres_basowy_groupBox.PerformLayout();
            this.znaki_chromatycze_groupBox.ResumeLayout(false);
            this.znaki_chromatycze_groupBox.PerformLayout();
            this.wartosci_rytmiczne_groupBox.ResumeLayout(false);
            this.wartosci_rytmiczne_groupBox.PerformLayout();
            this.pauzy_groupBox.ResumeLayout(false);
            this.pauzy_groupBox.PerformLayout();
            this.klucz_groupBox.ResumeLayout(false);
            this.klucz_groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox wiolinowy_od;
        private System.Windows.Forms.ComboBox wiolinowy_do;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button anuluj;
        private System.Windows.Forms.GroupBox zakres_wiolinowy_groupBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox klucz_combobox;
        private System.Windows.Forms.Label klucz_label;
        private System.Windows.Forms.GroupBox zakres_basowy_groupBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox basowy_od;
        private System.Windows.Forms.ComboBox basowy_do;
        private System.Windows.Forms.Label wiolinowy_alert_label;
        private System.Windows.Forms.Label basowy_alert_label;
        private System.Windows.Forms.GroupBox znaki_chromatycze_groupBox;
        private System.Windows.Forms.CheckBox brak_checkBox;
        private System.Windows.Forms.CheckBox krzyzyk_checkBox;
        private System.Windows.Forms.CheckBox bemol_checkBox;
        private System.Windows.Forms.Label znaki_chromatyczne_alert_label;
        private System.Windows.Forms.GroupBox wartosci_rytmiczne_groupBox;
        private System.Windows.Forms.CheckBox szesnastka_checkBox;
        private System.Windows.Forms.CheckBox osemka_checkBox;
        private System.Windows.Forms.CheckBox polnuta_checkBox;
        private System.Windows.Forms.CheckBox cwiercnuta_checkBox;
        private System.Windows.Forms.CheckBox cala_nuta_checkBox;
        private System.Windows.Forms.Label wartosci_rytmiczne_alert_label;
        private System.Windows.Forms.CheckBox podwojny_krzyzyk_checkBox;
        private System.Windows.Forms.CheckBox podwojny_bemol_checkBox;
        private System.Windows.Forms.GroupBox pauzy_groupBox;
        private System.Windows.Forms.Label symbole_ustawione_alert_label;
        private System.Windows.Forms.CheckBox kasownik_checkBox;
        private System.Windows.Forms.CheckBox pauza_szesnastkowa_checkBox;
        private System.Windows.Forms.CheckBox pauza_osemkowa_checkBox;
        private System.Windows.Forms.CheckBox pauza_polnutowa_checkBox;
        private System.Windows.Forms.CheckBox pauza_cwiercnutowa_checkBox;
        private System.Windows.Forms.CheckBox pauza_calonutowa_checkBox;
        private System.Windows.Forms.GroupBox klucz_groupBox;
    }
}