// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VaultSecretClient.cs" company="Orion Fusion">
// Copyright (c) Orion Fusion. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace KeyVaultSecretClient
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Azure.Identity;
    using Azure.Security.KeyVault.Secrets;
    using mykvscretclient;

    /// <inheritdoc />
    public class VaultSecretClient : ISecretClient
    {
        private readonly IVaultSecretClientConfig config;
        private readonly object locker = new object();
        private SecretClient secretClient;

        public VaultSecretClient(IVaultSecretClientConfig config)
        {
            this.config = config;
        }

        public string GetSecret(string key)
        {
            var client = this.GetSecretClient();
            if (client != null)
            {
                var response = client.GetSecret(key);
                if (response != null)
                {
                    return response.Value.Value;
                }
            }

            return null;
        }

        public async Task<string> GetSecretAsync(string key)
        {
            var client = this.GetSecretClient();
            if (client != null)
            {
                var response = await client.GetSecretAsync(key);
                if (response != null)
                {
                    return response.Value.Value;
                }
            }

            return null;
        }

        private static X509Certificate2 GetCertificate(string thumbprint)
        {
            // strip any non-hexadecimal values and make uppercase
            thumbprint = Regex.Replace(thumbprint, @"[^\da-fA-F]", string.Empty).ToUpper();
            var cert = FindByLocation(thumbprint, StoreLocation.CurrentUser);
            if (cert != null)
            {
                return cert;
            }

            cert = FindByLocation(thumbprint, StoreLocation.LocalMachine);
            if (cert != null)
            {
                return cert;
            }

            return null;
        }

        private SecretClient GetSecretClient()
        {
            lock (this.locker)
            {
                if (this.secretClient != null)
                {
                    return this.secretClient;
                }

                if (string.IsNullOrWhiteSpace(this.config.ClientId) ||
                    string.IsNullOrWhiteSpace(this.config.TenantId) ||
                    string.IsNullOrWhiteSpace(this.config.KeyVaultUri) ||
                    string.IsNullOrWhiteSpace(this.config.CertificateThumbprint))
                {
                    throw new ArgumentException("Config Values not set.");
                }

                var cert = GetCertificate(this.config.CertificateThumbprint);
                if (cert == null)
                {
                    throw new Exception("Certificate Not Found.");
                }

                var clientCertificateCredential = new ClientCertificateCredential(
                    this.config.TenantId,
                    this.config.ClientId,
                    cert);
                this.secretClient = new SecretClient(new Uri(this.config.KeyVaultUri), clientCertificateCredential);
                return this.secretClient;
            }
        }

        private static X509Certificate2 FindByLocation(string thumbprint, StoreLocation location)
        {
            var store = new X509Store(location);

            try
            {
                store.Open(OpenFlags.ReadOnly);

                var certCollection = store.Certificates;
                var signingCert = certCollection.Find(X509FindType.FindByThumbprint, thumbprint, false);
                if (signingCert.Count == 0)
                {
                    return null;
                }

                return signingCert[0];
            }
            finally
            {
                store.Close();
            }
        }
    }
}