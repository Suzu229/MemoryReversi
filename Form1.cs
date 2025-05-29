using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoryReversi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var board = new ReversiBoardControl();
            board.Location = new Point(10, 10);
            this.Controls.Add(board);

            this.ClientSize = board.Size;
        }
    }
}
