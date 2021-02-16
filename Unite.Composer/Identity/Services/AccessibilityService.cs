using System;
using System.IO;
using System.Linq;

namespace Unite.Composer.Identity.Services
{
    public class AccessibilityService : IAccessibilityService
    {
        private const string _directoryName = "data";
        private const string _fileName = "access-list.txt";

        private static readonly string _directoryPath = Path.Combine(Environment.CurrentDirectory, _directoryName);
        private static readonly string _filePath = Path.Combine(_directoryPath, _fileName);

        public AccessibilityService()
        {
            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }

            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Dispose();
            }
        }

        public bool IsConfigured()
        {
            var accessList = File.ReadAllLines(_filePath);

            return accessList.Any();
        }

        public bool IsAllowed(string email)
        {
            var accessList = File.ReadAllLines(_filePath);

            return accessList.Any(allowedEmail =>
            {
                var normalizedEmail = email.ToLower().Trim();
                var normalizedAllowedEmail = allowedEmail.ToLower().Trim();

                return string.Equals(normalizedEmail, normalizedAllowedEmail);
            });
        }
    }
}
