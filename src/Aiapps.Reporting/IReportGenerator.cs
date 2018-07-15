using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiapps.Reporting
{
    public interface IReportGenerator
    {
        byte[] Generate(string reportPath, byte[] logo, IDictionary<string, object[]> dataSources, string format, out string extension);
        byte[] Generate(string reportPath, byte[] logo, IDictionary<string, string> paramenters, IDictionary<string, object[]> dataSources, string format, out string extension);
        byte[] Generate(string reportPath, string logoPath, IDictionary<string, object[]> dataSources, string format, out string extension);
        byte[] Generate(string reportPath, string logoPath, IDictionary<string, string> paramenters, IDictionary<string, object[]> dataSources, string format, out string extension);
    }
}
