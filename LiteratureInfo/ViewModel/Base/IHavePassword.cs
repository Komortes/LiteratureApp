using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace LiteratureApp.Core
{
    public interface IHavePassword
    {
        SecureString SecurePassword { get; }

        SecureString SecurePasswordConf { get; }
    }
}
