using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    class PublicKey
    {
        public string key { get; set; }
        public string exp { get; set; }
        public string secret { get; set; }

    }

    class AcmeResponse<T>
    {
        public string Status { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }

    class EmployeeID
    {
        public int ID { get; set; }
    }

    class MissionResponseDetails
    {
        public string Assignment { get; set; }
        public string Response { get; set; }
    }

    class MissionDetails
    {
        public string EncryptedMessage { get; set; }
        public string EncryptedKey { get; set; }
        public string MissionHash { get; set; }
        public string IV { get; set; }
        public string Signature { get; set; }
    }

    class MissionResponse
    {
        public int id { get; set; }
        public string key { get; set; }
        public string iv { get; set; }
        public string msg { get; set; }
        public string sig { get; set; }
    }

    class FinishMissionResponse
    {
        public string Response { get; set; }
    }

    class RsaEncDec
    {
        public RSAParameters MyPrivateKey { get; set; }
        public RSAParameters MyPublicKey { get; set; }
        public int EmpID { get; set; }
        public RSAParameters AcmePublicKey { get; set; }
        public string DecryptedMessageResponse { get; set; }
        public string MissionResponse { get; set; }


        public void GeneratePrivatePublicKey()
        {
            if (!File.Exists("privatekey.xml"))
            {
                GenerateKeys();
                SaveKeys();
            }
            else
            {
                LoadKeys();
            }

        }

        public void GetEmployeeID()
        {
            if (!File.Exists("empid.txt"))
            {
                string hashsecret = HashSecret();
                UploadKey(hashsecret);
            }
            else
            {
                LoadEmpID();
            }
        }

        public void VerifyMissionDetails()
        {
            MissionDetails missionDetails = GetMissionDetails();
            VerifySignature(missionDetails);
            string decryptedMessage = DecryptKey(missionDetails);
            DecryptedMessageResponse = GetResponse(decryptedMessage);
        }

        private MissionDetails GetMissionDetails()
        {
            const string url = @"http://cst407.azurewebsites.net/Lab6/GetMissionDetails";
            WebClient webClient = new WebClient();
            string responsejson = webClient.DownloadString(url + "?id=" + EmpID);

            AcmeResponse<MissionDetails> response = JsonConvert.DeserializeObject<AcmeResponse<MissionDetails>>(responsejson);

            if (response.Status == "OK")
            {
                return response.Data;
            }
            else
            {
                throw new Exception(response.Message);
            }
        }

        private string GetResponse(string message)
        {
            MissionResponseDetails details = JsonConvert.DeserializeObject<MissionResponseDetails>(message);

            return details.Response;
        }

        public void UploadMissionDetails()
        {

            byte[] key = new byte[32];
            byte[] iv = new byte[16];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(key);
                rng.GetBytes(iv);
            }

            Aes aes = Aes.Create();

            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor();


            byte[] encryptedMessageBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    byte[] decryptedMissionBytes = DecryptedMessageResponse.GetBytes();
                    cs.Write(decryptedMissionBytes, 0, decryptedMissionBytes.Length);
                }

                encryptedMessageBytes = ms.ToArray();
            }

            string encryptedMessage = encryptedMessageBytes.ToBase64();

            RSA rsa = RSA.Create(2048);

            rsa.ImportParameters(AcmePublicKey);

            byte[] encryptedKeyBytes = rsa.Encrypt(key, RSAEncryptionPadding.Pkcs1);

            string encryptedKey = encryptedKeyBytes.ToBase64();

            rsa.ImportParameters(MyPrivateKey);

            byte[] signatureBytes = rsa.SignData(encryptedMessageBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            string signature = signatureBytes.ToBase64();

            MissionResponse missionresponse = new MissionResponse
            {
                id = EmpID,
                key = encryptedKey,
                iv = iv.ToBase64(),
                msg = encryptedMessage,
                sig = signature
            };

            string json = JsonConvert.SerializeObject(missionresponse);

            const string url = @"http://cst407.azurewebsites.net/Lab6/FinishMission";

            WebClient webClient = new WebClient();
            
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            string responsejson = webClient.UploadString(url, json);

            AcmeResponse<FinishMissionResponse> response = JsonConvert.DeserializeObject<AcmeResponse<FinishMissionResponse>>(responsejson);

            if (response.Status == "OK")
            {
                MissionResponse = response.Data.Response;
            }
            else
            {
                throw new Exception(response.Message);
            }
        }

        private void SaveKeys()
        {
            RSA rsa = RSA.Create(2048);

            rsa.ImportParameters(MyPrivateKey);

            string xml = rsa.ToXmlString(true);

            File.WriteAllText("privatekey.xml", xml);
        }

        private void GenerateKeys()
        {
            RSA rsa = RSA.Create(2048);

            MyPrivateKey = rsa.ExportParameters(true);
            MyPublicKey = rsa.ExportParameters(false);
        }

        private void LoadKeys()
        {
            string xml = File.ReadAllText("privatekey.xml");

            RSA rsa = RSA.Create();

            rsa.FromXmlString(xml);
            MyPrivateKey = rsa.ExportParameters(true);
            MyPublicKey = rsa.ExportParameters(false);
        }

        private string HashSecret()
        {
            const string secretText = "lab6";
            SHA256 sha = SHA256.Create();

            byte[] hash = sha.ComputeHash(secretText.GetBytes());

            HMACSHA256 hmac = new HMACSHA256(hash);

            //    string message = "hello";
            byte[] hashedmod = hmac.ComputeHash(MyPublicKey.Modulus);

            return hashedmod.ToHexString();
        }

        private void UploadKey(string hashsecret)
        {
            PublicKey keytoupload = new PublicKey
            {
                key = MyPublicKey.Modulus.ToBase64(),
                exp = MyPublicKey.Exponent.ToBase64(),
                secret = hashsecret
            };

            string json = JsonConvert.SerializeObject(keytoupload);

            const string url = @"http://cst407.azurewebsites.net/Lab6/UploadPubKey";

            WebClient webClient = new WebClient();

            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            string responsejson = webClient.UploadString(url, json);

            AcmeResponse<EmployeeID> response = JsonConvert.DeserializeObject<AcmeResponse<EmployeeID>>(responsejson);

            if(response.Status=="OK")
            {
                EmpID = response.Data.ID;
                SaveEmpID();
            }
            else
            {
                throw new Exception(response.Message);
            }
        }

        private void SaveEmpID()
        {
            File.WriteAllText("empid.txt", EmpID.ToString());
        }

        private void LoadEmpID()
        {
            EmpID = int.Parse(File.ReadAllText("empid.txt"));
        }

        private void VerifySignature(MissionDetails missionDetails)
        {
            byte[] encryptedMessage = missionDetails.EncryptedMessage.FromBase64();

            SHA256 sha = SHA256.Create();

            byte[] hash = sha.ComputeHash(encryptedMessage);

            string xml = File.ReadAllText("ACMEpublickey.xml");

            RSA rsa = RSA.Create(2048);

            rsa.FromXmlString(xml);
            AcmePublicKey = rsa.ExportParameters(false);
            bool result = rsa.VerifyHash(hash, missionDetails.Signature.FromBase64(), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            if (result == false)
            {
                throw new Exception("Failed Signature");
            }
        }

        private string DecryptKey(MissionDetails missionDetails)
        {

            byte[] encryptedkey = missionDetails.EncryptedKey.FromBase64();
            byte[] decryptedkey;

            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(MyPrivateKey);
                decryptedkey = rsa.Decrypt(encryptedkey, RSAEncryptionPadding.Pkcs1);
            }

            Aes aes = Aes.Create();

            aes.Key = decryptedkey;

            aes.IV = missionDetails.IV.FromBase64();

            ICryptoTransform decryptor = aes.CreateDecryptor();


            byte[] decryptedMessageBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                {
                    byte[] encryptedMissionBytes = missionDetails.EncryptedMessage.FromBase64();
                    cs.Write(encryptedMissionBytes, 0, encryptedMissionBytes.Length);
                }

                decryptedMessageBytes = ms.ToArray();
            }

            string decryptedMessage = decryptedMessageBytes.GetString();

            VerifyMissionString(decryptedMessage, missionDetails.MissionHash);

            return decryptedMessage;
        }

        private void VerifyMissionString(string decryptedMessage, string missionHash)
        {
            SHA256 sha = SHA256.Create();

            byte[] hash = sha.ComputeHash(decryptedMessage.GetBytes());

            bool result = (hash.ToHexString() == missionHash);

            if (!result)
            {
                throw new Exception("message hash mismatch!");
            }
        }

    }
}
