using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class EmailTemplates
    {
        public static string NewUserWithActivation = @"<table>
            <tr>
                <td>
                    Welcome to eskrima! Please use this code :  <h3 style='color: blue;'>[activationCode]</h3>
                </td>
            </tr>
			<tr>
				<td>
					 to fully activate your account sent to your email : <h3 style='color: blue;'>[emailAdd]</h3>
				</td>
			</tr>
            <tr>
                <td>
                    Thanks,
                </td>
            </tr>
            <tr>
                <td>
                    Eksrima
                </td>
            </tr>
        </table>";
    }
}
