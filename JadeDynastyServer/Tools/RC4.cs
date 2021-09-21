using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JadeDynastyServer.Tools
{
    class RC4
    {
        int x, y;
        byte[] state;

        public RC4(byte[] key)
        {
            x = 0;
            y = 0;
            state = EncryptInitalize(key);
        }

        public byte[] Encrypt(byte[] data)
        {
            var bystes = EncryptOutput(data).ToArray();
            return bystes;
        }

        private byte[] EncryptInitalize(byte[] key)
        {
            byte[] s = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();

            for (int i = 0, j = 0; i < 256; i++)
            {
                j = (j + key[i % key.Length] + s[i]) & 255;

                Swap(s, i, j);
            }

            return s;
        }

        private IEnumerable<byte> EncryptOutput(IEnumerable<byte> data)
        {
            return data.Select((b) =>
            {
                x = (x + 1) & 255;
                y = (y + state[x]) & 255;

                Swap(state, x, y);

                return (byte)(b ^ state[(state[x] + state[y]) & 255]);
            });
        }

        private void Swap(byte[] s, int i, int j)
        {
            byte c = s[i];
            s[i] = s[j];
            s[j] = c;
        }
    }
}
