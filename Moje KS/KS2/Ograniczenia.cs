using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace KS2
{
    

    public partial class OknoGlowne : Form
    {


        private void UstawMenu()
        {
            if (Parametry.Ograniczenie.edycjaProgramu == "uczniowie")
            {
                this.edytorInterwałówToolStripMenuItem.Visible = false;
                this.materiałyToolStripMenuItem1.Visible = false;
                this.tonacjeToolStripMenuItem.Visible = false;
                this.edytorToolStripMenuItem.Visible = false;
                this.materiałyToolStripMenuItem.Visible = false; //losowe
                //this.rozpoznawanieInterwałówPięcioliniaToolStripMenuItem.Visible = false;
            }
        }

    }
}
