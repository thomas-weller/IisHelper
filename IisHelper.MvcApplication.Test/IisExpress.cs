using System;
using System.Diagnostics;
using System.IO;

namespace IisHelper.MvcApplication.Test
{
    /// <summary>
    /// Helper class for running IIS Express. Basically a convenient wrapper around IIS Express' command line.
    /// </summary>
    public class IisExpress
    {
        #region Fields

        private Process _process;

        #endregion // Fields

        #region Properties

        /// <summary>
        /// The path to the <c>iisexpress.exe</c> executable. Defaults to the default installation path
        /// (e.g. <c>C:\Program Files (x86)\IIS Express\iisexpress.exe</c>), if not explicitely set.
        /// </summary>
        public string ExePath { get; set; }

        /// <summary>
        /// Path to the IIS Express configuration file. Defaults to <c>USER\DOCUMENTS\IISExpress\config\applicationhost.config</c>.
        /// </summary>
        public string ConfigPath { get; set; }

        /// <summary>
        /// The application pool that IIS Express should use. Defaults to <c>"Clr4IntegratedAppPool"</c>.
        /// </summary>
        public string AppPool { get; set; }

        /// <summary>
        /// Gets a value indicating whether the associated IIS Express instance is currently running.
        /// </summary>
        public bool IsRunning { get { return _process != null; } }

        #endregion // Properties

        #region Operations

        /// <summary>
        /// Starts IIS Express with the specified website.
        /// </summary>
        /// <param name="site">The website that IIS Express should host.</param>
        /// <exception cref="ArgumentNullException">
        /// Argument <paramref name="site"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// There already is an associated IIS Express instance running (i.e. <see cref="IsRunning"/> is true).
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// Either <see cref="ExePath"/> or <see cref="ConfigPath"/> could not be found.
        /// </exception>
        public void Start(string site)
        {
            if (site == null)
            {
                throw new ArgumentNullException("site");
            }

            if (_process != null)
            {
                throw new InvalidOperationException("IIS Express is already running.");
            }

            SetDefaultsWhereNecessary();

            if (!File.Exists(ExePath))
            {
                throw new FileNotFoundException("Path to IIS Express executable is invalid: '" + ExePath + "'.");
            }

            if (string.IsNullOrEmpty(ConfigPath) || !File.Exists(ConfigPath))
            {
                throw new FileNotFoundException("Path to IIS Express configuration file is invalid: '" + ConfigPath + "'.");
            }

            _process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ExePath,
                    Arguments = string.Format(
                        "/config:\"{0}\" /site:\"{1}\" /apppool:\"{2}\"",
                        Path.GetFullPath(ConfigPath),
                        site,
                        AppPool),
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            _process.Start();
        }

        /// <summary>
        /// Stops the IIS Express instance that was formerly started via the <see cref="Start"/> method.
        /// </summary>
        public void Stop()
        {
            if (_process != null)
            {
                _process.Kill();
                _process.WaitForExit();
                _process = null;
            }
        }

        #endregion // Operations

        #region Implementation

        private void SetDefaultsWhereNecessary()
        {
            if (string.IsNullOrEmpty(ExePath))
            {
                ExePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    @"IIS Express\iisexpress.exe");
            }

            if (string.IsNullOrEmpty(ConfigPath))
            {
                ConfigPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    @"IISExpress\config\applicationhost.config");
            }

            if (string.IsNullOrEmpty(AppPool))
            {
                AppPool = "Clr4IntegratedAppPool";
            }
        }

        #endregion // Implementation
    }
}