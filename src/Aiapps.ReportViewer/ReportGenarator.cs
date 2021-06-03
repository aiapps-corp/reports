using Aiapps.Reporting;
using Common.Logging;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiapps.ReportViewer
{
    public class ReportGenarator : IReportGenerator
    {
        private ILog _logger = LogManager.GetLogger<ReportGenarator>();

        public byte[] Generate(string reportPath, string logoPath, IDictionary<string, object[]> dataSources, string format, out string extension)
        {
            return Generate(reportPath,
                logoPath,
                new Dictionary<string, string>(),
                dataSources,
                format,
                out extension);
        }

        public byte[] Generate(string reportPath, byte[] logo, IDictionary<string, object[]> dataSources, string format, out string extension)
        {
            var logoBase64 = "No Logo";
            if (logo != null)
                logoBase64 = Convert.ToBase64String(logo);
            return Generate(reportPath,
                logoBase64,
                new Dictionary<string, string>(),
                dataSources,
                format,
                out extension);
        }
        public byte[] Generate(string reportPath, byte[] logo, IDictionary<string, string> paramenters, IDictionary<string, object[]> dataSources, string format, out string extension)
        {

            var logoBase64 = "No Logo";
            if (logo != null)
                logoBase64 = Convert.ToBase64String(logo);
            return Generate(reportPath,
                logoBase64,
                paramenters,
                dataSources,
                format,
                out extension);
        }

        public byte[] Generate(string reportPath, string logo, IDictionary<string, string> paramenters, IDictionary<string, object[]> dataSources, string format, out string extension)
        {
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;

                var viewer = new Microsoft.Reporting.WebForms.ReportViewer();
                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.EnableHyperlinks = true;
                viewer.LocalReport.ReportPath = reportPath;
                _logger.Debug($"Report Path: {reportPath}");
                viewer.LocalReport.EnableExternalImages = true;

                foreach (var d in paramenters)
                {
                    _logger.Debug($"Start SetParameters: {d.Key}");
                    viewer.LocalReport.SetParameters(new ReportParameter(d.Key, d.Value));
                    _logger.Debug($"End SetParameters: {d.Key}");
                }

                ConfigureLogoParameter(logo, viewer);

                foreach (var d in dataSources)
                {
                    var reportDataSource = new ReportDataSource(d.Key, d.Value);
                    viewer.LocalReport.DataSources.Add(reportDataSource);
                }
                _logger.Debug($"Start Render report: {reportPath}");
                byte[] bytes = viewer.LocalReport.Render(
                        format, null, out mimeType, out encoding, out extension,
                        out streamids, out warnings);
                _logger.Debug($"End Render report: {reportPath}");
                return bytes;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw;
            }
        }

        private void ConfigureLogoParameter(string logo, Microsoft.Reporting.WebForms.ReportViewer viewer)
        {
            if (!string.IsNullOrWhiteSpace(logo))
            {
                _logger.Debug($"Start SetParameters ImagePath: {string.Join("", logo.Take(250))}");
                if (logo.Contains("http"))
                {
                    viewer.LocalReport.SetParameters(new ReportParameter("ImagePath", logo));
                }
                else
                {
                    viewer.LocalReport.SetParameters(new ReportParameter("ReportLogo", logo));
                }
                _logger.Debug($"End SetParameters ImagePath: {string.Join("", logo.Take(250))}");
            }
        }
    }
}
