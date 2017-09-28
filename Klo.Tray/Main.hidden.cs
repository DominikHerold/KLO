using System.Windows.Forms;

namespace Klo.Tray
{
    partial class Main
    {
        // To really *hide* the Form window (via https://stackoverflow.com/a/17893626/385507)
        protected override CreateParams CreateParams
        {
            get
            {
                var @params = base.CreateParams;
                @params.ExStyle |= 0x80;
                return @params;
            }
        }
    }
}
