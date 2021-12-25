using System;
namespace GreenOnion.Services
{
    public class IDToIntGenerator
    {

        private byte[] buffer = Guid.NewGuid().ToByteArray();

        public IDToIntGenerator()
        {
        }

        public Int64 Generate()
        {
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}
