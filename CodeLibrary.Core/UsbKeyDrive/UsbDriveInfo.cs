using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace CodeLibrary.Core
{
    internal class UsbDriveInfo
    {
        private const string KEY_DIR_NAME = "_USBKey";

        public string CreateKey(out UsbKeyDriveErrorEnum error)
        {
            string _id = Guid.NewGuid().ToString().Replace("-", "");
            byte[] _key = Generate256BitsOfRandomEntropy();

            DriveInfo _drive = GetUsbKeyDrives().FirstOrDefault();
            string _path = Path.Combine(UsbKeyPath(_drive), _id);
            try
            {
                File.WriteAllBytes(_path, _key);
                error = UsbKeyDriveErrorEnum.Ok;
                return _id;
            }
            catch
            {
            }
            error = UsbKeyDriveErrorEnum.ExceptionWritingKeyFile;
            return null;
        }

        private void SyncKey(string id, byte[] key)
        {
            UsbDriveInfo _driveInfo = new UsbDriveInfo();
            if (_driveInfo.GetUsbKeyDrives().Count < 2)
            {
                return;
            }

            foreach (DriveInfo _drive in GetUsbKeyDrives())
            {
                string _path = Path.Combine(UsbKeyPath(_drive), id);
                if (!File.Exists(_path))
                {
                    try
                    {
                        File.WriteAllBytes(_path, key);
                    }
                    catch
                    { }
                }
            }
        }


        public bool CreateUsbKeyDrive(DriveInfo driveInfo, out UsbKeyDriveErrorEnum error)
        {
            if (IsUsbKeyDrive(driveInfo))
            {
                error = UsbKeyDriveErrorEnum.Ok;
                return true;
            }
            try
            {
                string _path = UsbKeyPath(driveInfo);
                Directory.CreateDirectory(_path);
                error = UsbKeyDriveErrorEnum.Ok;
                return true;
            }
            catch (Exception)
            {
                error = UsbKeyDriveErrorEnum.ExceptionCreatingKeyDrive;
            }

            return false;
        }

        public byte[] GetKey(string id, out UsbKeyDriveErrorEnum error)
        {
            error = UsbKeyDriveErrorEnum.CannotFindKeyOnUsbKeyDrive;

            foreach (DriveInfo _drive in GetUsbKeyDrives())
            {
                string _path = Path.Combine(UsbKeyPath(_drive), id);

                if (!File.Exists(_path))
                {
                    continue;
                }

                try
                {
                    byte[] _key = File.ReadAllBytes(_path);
                    error = UsbKeyDriveErrorEnum.Ok;
                    SyncKey(id, _key);
                    return _key;
                }
                catch (Exception)
                {
                    error = UsbKeyDriveErrorEnum.ExceptionReadingKeyFile;
                    continue;
                }
            }

            return null;
        }

        public List<DriveInfo> GetUsbDrives()
        {
            return DriveInfo.GetDrives().Where(p => p.DriveType == DriveType.Removable).ToList();
        }

        public List<DriveInfo> GetUsbKeyDrives()
        {
            return DriveInfo.GetDrives().Where(p => p.DriveType == DriveType.Removable).Where(p => IsUsbKeyDrive(p) == true).ToList();
        }

        public bool IsUsbKeyDrive(DriveInfo driveInfo)
        {
            string _path = UsbKeyPath(driveInfo);
            return (Directory.Exists(_path));
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[128]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        private string UsbKeyPath(DriveInfo driveInfo) => Path.Combine(driveInfo.RootDirectory.FullName, KEY_DIR_NAME);
    }
}