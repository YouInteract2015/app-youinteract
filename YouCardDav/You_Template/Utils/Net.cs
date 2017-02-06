using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace You_Contacts.Utils
{
    public class Net
    {
        #region static method GetLocalHostName

        public static string GetLocalHostName(string hostName)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                return System.Net.Dns.GetHostName();
            }
            else
            {
                return hostName;
            }
        }

        #endregion

        #region static method CompareArray

        public static bool CompareArray(Array array1, Array array2)
        {
            return CompareArray(array1, array2, array2.Length);
        }

        public static bool CompareArray(Array array1, Array array2, int array2Count)
        {
            if (array1 == null && array2 == null)
            {
                return true;
            }
            if (array1 == null && array2 != null)
            {
                return false;
            }
            if (array1 != null && array2 == null)
            {
                return false;
            }
            if (array1.Length != array2Count)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < array1.Length; i++)
                {
                    if (!array1.GetValue(i).Equals(array2.GetValue(i)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion

        #region static method ReverseArray

        public static Array ReverseArray(Array array)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            Array.Reverse(array);

            return array;
        }

        #endregion

        #region static method ArrayToString

        public static string ArrayToString(string[] values, string delimiter)
        {
            if (values == null)
            {
                return "";
            }

            StringBuilder retVal = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                if (i > 0)
                {
                    retVal.Append(delimiter);
                }

                retVal.Append(values[i]);
            }

            return retVal.ToString();
        }

        #endregion

        #region static method StreamCopy

        public static long StreamCopy(Stream source, Stream target, int blockSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            if (blockSize < 1024)
            {
                throw new ArgumentException("Argument 'blockSize' value must be >= 1024.");
            }

            byte[] buffer = new byte[blockSize];
            long totalReaded = 0;
            while (true)
            {
                int readedCount = source.Read(buffer, 0, buffer.Length);
                // We reached end of stream, we readed all data sucessfully.
                if (readedCount == 0)
                {
                    return totalReaded;
                }
                else
                {
                    target.Write(buffer, 0, readedCount);
                    totalReaded += readedCount;
                }
            }
        }

        #endregion


        #region static method CompareIP

        public static int CompareIP(IPAddress source, IPAddress destination)
        {
            byte[] sourceIpBytes = source.GetAddressBytes();
            byte[] destinationIpBytes = destination.GetAddressBytes();

            // IPv4 and IPv6
            if (sourceIpBytes.Length < destinationIpBytes.Length)
            {
                return 1;
            }
            // IPv6 and IPv4
            else if (sourceIpBytes.Length > destinationIpBytes.Length)
            {
                return -1;
            }
            // IPv4 and IPv4 OR IPv6 and IPv6
            else
            {
                for (int i = 0; i < sourceIpBytes.Length; i++)
                {
                    if (sourceIpBytes[i] < destinationIpBytes[i])
                    {
                        return 1;
                    }
                    else if (sourceIpBytes[i] > destinationIpBytes[i])
                    {
                        return -1;
                    }
                }

                return 0;
            }
        }

        #endregion

        #region static method IsIPAddress

        public static bool IsIPAddress(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            IPAddress ip = null;

            return IPAddress.TryParse(value, out ip);
        }

        #endregion

        #region static method IsMulticastAddress

        public static bool IsMulticastAddress(IPAddress ip)
        {
            if (ip == null)
            {
                throw new ArgumentNullException("ip");
            }

            // IPv4 multicast 224.0.0.0 to 239.255.255.255

            if (ip.IsIPv6Multicast)
            {
                return true;
            }
            else if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                byte[] bytes = ip.GetAddressBytes();
                if (bytes[0] >= 224 && bytes[0] <= 239)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region static method IsPrivateIP

        public static bool IsPrivateIP(string ip)
        {
            if (ip == null)
            {
                throw new ArgumentNullException("ip");
            }

            return IsPrivateIP(IPAddress.Parse(ip));
        }

        public static bool IsPrivateIP(IPAddress ip)
        {
            if (ip == null)
            {
                throw new ArgumentNullException("ip");
            }

            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                byte[] ipBytes = ip.GetAddressBytes();

                if (ipBytes[0] == 192 && ipBytes[1] == 168)
                {
                    return true;
                }
                if (ipBytes[0] == 172 && ipBytes[1] >= 16 && ipBytes[1] <= 31)
                {
                    return true;
                }
                if (ipBytes[0] == 10)
                {
                    return true;
                }
                if (ipBytes[0] == 169 && ipBytes[1] == 254)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region static method ParseIPEndPoint

        public static IPEndPoint ParseIPEndPoint(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            try
            {
                string[] ip_port = value.Split(':');

                return new IPEndPoint(IPAddress.Parse(ip_port[0]), Convert.ToInt32(ip_port[1]));
            }
            catch (Exception x)
            {
                throw new ArgumentException("Invalid IPEndPoint value.", "value", x);
            }
        }

        #endregion


        #region static method IsInteger

        public static bool IsInteger(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            long l = 0;

            return long.TryParse(value, out l);
        }

        #endregion

        #region static method IsAscii

        public static bool IsAscii(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            foreach (char c in value)
            {
                if ((int)c > 127)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion


        #region static method IsSocketAsyncSupported

        public static bool IsSocketAsyncSupported()
        {
            try
            {
                using (SocketAsyncEventArgs e = new SocketAsyncEventArgs())
                {
                    return true;
                }
            }
            catch (NotSupportedException nX)
            {
                string dummy = nX.Message;

                return false;
            }
        }

        #endregion

        #region static method CreateSocket

        public static Socket CreateSocket(IPEndPoint localEP, ProtocolType protocolType)
        {
            if (localEP == null)
            {
                throw new ArgumentNullException("localEP");
            }

            SocketType socketType = SocketType.Stream;
            if (protocolType == ProtocolType.Udp)
            {
                socketType = SocketType.Dgram;
            }

            if (localEP.AddressFamily == AddressFamily.InterNetwork)
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, socketType, protocolType);
                socket.Bind(localEP);

                return socket;
            }
            else if (localEP.AddressFamily == AddressFamily.InterNetworkV6)
            {
                Socket socket = new Socket(AddressFamily.InterNetworkV6, socketType, protocolType);
                socket.Bind(localEP);

                return socket;
            }
            else
            {
                throw new ArgumentException("Invalid IPEndPoint address family.");
            }
        }

        #endregion


        #region static method ToHex

        public static string ToHex(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            return BitConverter.ToString(data).ToLower().Replace("-", "");
        }
        public static string ToHex(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return BitConverter.ToString(Encoding.Default.GetBytes(text)).ToLower().Replace("-", "");
        }

        #endregion

        #region static method FromHex

        public static byte[] FromHex(byte[] hexData)
        {
            if (hexData == null)
            {
                throw new ArgumentNullException("hexData");
            }

            if (hexData.Length < 2 || (hexData.Length / (double)2 != Math.Floor(hexData.Length / (double)2)))
            {
                throw new Exception("Illegal hex data, hex data must be in two bytes pairs, for example: 0F,FF,A3,... .");
            }

            MemoryStream retVal = new MemoryStream(hexData.Length / 2);
            // Loop hex value pairs
            for (int i = 0; i < hexData.Length; i += 2)
            {
                byte[] hexPairInDecimal = new byte[2];
                // We need to convert hex char to decimal number, for example F = 15
                for (int h = 0; h < 2; h++)
                {
                    if (((char)hexData[i + h]) == '0')
                    {
                        hexPairInDecimal[h] = 0;
                    }
                    else if (((char)hexData[i + h]) == '1')
                    {
                        hexPairInDecimal[h] = 1;
                    }
                    else if (((char)hexData[i + h]) == '2')
                    {
                        hexPairInDecimal[h] = 2;
                    }
                    else if (((char)hexData[i + h]) == '3')
                    {
                        hexPairInDecimal[h] = 3;
                    }
                    else if (((char)hexData[i + h]) == '4')
                    {
                        hexPairInDecimal[h] = 4;
                    }
                    else if (((char)hexData[i + h]) == '5')
                    {
                        hexPairInDecimal[h] = 5;
                    }
                    else if (((char)hexData[i + h]) == '6')
                    {
                        hexPairInDecimal[h] = 6;
                    }
                    else if (((char)hexData[i + h]) == '7')
                    {
                        hexPairInDecimal[h] = 7;
                    }
                    else if (((char)hexData[i + h]) == '8')
                    {
                        hexPairInDecimal[h] = 8;
                    }
                    else if (((char)hexData[i + h]) == '9')
                    {
                        hexPairInDecimal[h] = 9;
                    }
                    else if (((char)hexData[i + h]) == 'A' || ((char)hexData[i + h]) == 'a')
                    {
                        hexPairInDecimal[h] = 10;
                    }
                    else if (((char)hexData[i + h]) == 'B' || ((char)hexData[i + h]) == 'b')
                    {
                        hexPairInDecimal[h] = 11;
                    }
                    else if (((char)hexData[i + h]) == 'C' || ((char)hexData[i + h]) == 'c')
                    {
                        hexPairInDecimal[h] = 12;
                    }
                    else if (((char)hexData[i + h]) == 'D' || ((char)hexData[i + h]) == 'd')
                    {
                        hexPairInDecimal[h] = 13;
                    }
                    else if (((char)hexData[i + h]) == 'E' || ((char)hexData[i + h]) == 'e')
                    {
                        hexPairInDecimal[h] = 14;
                    }
                    else if (((char)hexData[i + h]) == 'F' || ((char)hexData[i + h]) == 'f')
                    {
                        hexPairInDecimal[h] = 15;
                    }
                }

                // Join hex 4 bit(left hex cahr) + 4bit(right hex char) in bytes 8 it
                retVal.WriteByte((byte)((hexPairInDecimal[0] << 4) | hexPairInDecimal[1]));
            }

            return retVal.ToArray();
        }

        #endregion


        #region static method FromBase64

        public static byte[] FromBase64(string data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            return Convert.FromBase64String(data);
        }

        public static byte[] FromBase64(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            return Encoding.ASCII.GetBytes(Convert.ToBase64String(data));
        }

        #endregion

        #region static method Base64Encode

        public static byte[] Base64Encode(byte[] data)
        {
            return Base64EncodeEx(data, null, true);
        }

        public static byte[] Base64EncodeEx(byte[] data, char[] base64Chars, bool padd)
        {
            /* RFC 2045 6.8.  Base64 Content-Transfer-Encoding
			
                Base64 is processed from left to right by 4 6-bit byte block, 4 6-bit byte block 
                are converted to 3 8-bit bytes.
                If base64 4 byte block doesn't have 3 8-bit bytes, missing bytes are marked with =. 
				
			
                Value Encoding  Value Encoding  Value Encoding  Value Encoding
                    0 A            17 R            34 i            51 z
                    1 B            18 S            35 j            52 0
                    2 C            19 T            36 k            53 1
                    3 D            20 U            37 l            54 2
                    4 E            21 V            38 m            55 3
                    5 F            22 W            39 n            56 4
                    6 G            23 X            40 o            57 5
                    7 H            24 Y            41 p            58 6
                    8 I            25 Z            42 q            59 7
                    9 J            26 a            43 r            60 8
                    10 K           27 b            44 s            61 9
                    11 L           28 c            45 t            62 +
                    12 M           29 d            46 u            63 /
                    13 N           30 e            47 v
                    14 O           31 f            48 w         (pad) =
                    15 P           32 g            49 x
                    16 Q           33 h            50 y
					
                NOTE: 4 base64 6-bit bytes = 3 8-bit bytes				
                    // |    6-bit    |    6-bit    |    6-bit    |    6-bit    |
                    // | 1 2 3 4 5 6 | 1 2 3 4 5 6 | 1 2 3 4 5 6 | 1 2 3 4 5 6 |
                    // |    8-bit         |    8-bit        |    8-bit         |
            */

            if (base64Chars != null && base64Chars.Length != 64)
            {
                throw new Exception("There must be 64 chars in base64Chars char array !");
            }

            if (base64Chars == null)
            {
                base64Chars = new char[]{
					'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
					'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
					'0','1','2','3','4','5','6','7','8','9','+','/'
				};
            }

            // Convert chars to bytes
            byte[] base64LoockUpTable = new byte[64];
            for (int i = 0; i < 64; i++)
            {
                base64LoockUpTable[i] = (byte)base64Chars[i];
            }

            int encodedDataLength = (int)Math.Ceiling((data.Length * 8) / (double)6);
            // Retrun value won't be interegral 4 block, but has less. Padding requested, padd missing with '='
            if (padd && (encodedDataLength / (double)4 != Math.Ceiling(encodedDataLength / (double)4)))
            {
                encodedDataLength += (int)(Math.Ceiling(encodedDataLength / (double)4) * 4) - encodedDataLength;
            }

            // See how many line brakes we need
            int numberOfLineBreaks = 0;
            if (encodedDataLength > 76)
            {
                numberOfLineBreaks = (int)Math.Ceiling(encodedDataLength / (double)76) - 1;
            }

            // Construc return valu buffer
            byte[] retVal = new byte[encodedDataLength + (numberOfLineBreaks * 2)];  // * 2 - CRLF

            int lineBytes = 0;
            // Loop all 3 bye blocks
            int position = 0;
            for (int i = 0; i < data.Length; i += 3)
            {
                // Do line splitting
                if (lineBytes >= 76)
                {
                    retVal[position + 0] = (byte)'\r';
                    retVal[position + 1] = (byte)'\n';
                    position += 2;
                    lineBytes = 0;
                }

                // Full 3 bytes data block
                if ((data.Length - i) >= 3)
                {
                    retVal[position + 0] = base64LoockUpTable[data[i + 0] >> 2];
                    retVal[position + 1] = base64LoockUpTable[(data[i + 0] & 0x3) << 4 | data[i + 1] >> 4];
                    retVal[position + 2] = base64LoockUpTable[(data[i + 1] & 0xF) << 2 | data[i + 2] >> 6];
                    retVal[position + 3] = base64LoockUpTable[data[i + 2] & 0x3F];
                    position += 4;
                    lineBytes += 4;
                }
                // 2 bytes data block, left (last block)
                else if ((data.Length - i) == 2)
                {
                    retVal[position + 0] = base64LoockUpTable[data[i + 0] >> 2];
                    retVal[position + 1] = base64LoockUpTable[(data[i + 0] & 0x3) << 4 | data[i + 1] >> 4];
                    retVal[position + 2] = base64LoockUpTable[(data[i + 1] & 0xF) << 2];
                    if (padd)
                    {
                        retVal[position + 3] = (byte)'=';
                    }
                }
                // 1 bytes data block, left (last block)
                else if ((data.Length - i) == 1)
                {
                    retVal[position + 0] = base64LoockUpTable[data[i + 0] >> 2];
                    retVal[position + 1] = base64LoockUpTable[(data[i + 0] & 0x3) << 4];
                    if (padd)
                    {
                        retVal[position + 2] = (byte)'=';
                        retVal[position + 3] = (byte)'=';
                    }
                }
            }

            return retVal;
        }

        #endregion

        #region static method Base64Decode

        public static byte[] Base64DecodeEx(byte[] base64Data, char[] base64Chars)
        {
            /* RFC 2045 6.8.  Base64 Content-Transfer-Encoding
			
                Base64 is processed from left to right by 4 6-bit byte block, 4 6-bit byte block 
                are converted to 3 8-bit bytes.
                If base64 4 byte block doesn't have 3 8-bit bytes, missing bytes are marked with =. 
				
			
                Value Encoding  Value Encoding  Value Encoding  Value Encoding
                    0 A            17 R            34 i            51 z
                    1 B            18 S            35 j            52 0
                    2 C            19 T            36 k            53 1
                    3 D            20 U            37 l            54 2
                    4 E            21 V            38 m            55 3
                    5 F            22 W            39 n            56 4
                    6 G            23 X            40 o            57 5
                    7 H            24 Y            41 p            58 6
                    8 I            25 Z            42 q            59 7
                    9 J            26 a            43 r            60 8
                    10 K           27 b            44 s            61 9
                    11 L           28 c            45 t            62 +
                    12 M           29 d            46 u            63 /
                    13 N           30 e            47 v
                    14 O           31 f            48 w         (pad) =
                    15 P           32 g            49 x
                    16 Q           33 h            50 y
					
                NOTE: 4 base64 6-bit bytes = 3 8-bit bytes				
                    // |    6-bit    |    6-bit    |    6-bit    |    6-bit    |
                    // | 1 2 3 4 5 6 | 1 2 3 4 5 6 | 1 2 3 4 5 6 | 1 2 3 4 5 6 |
                    // |    8-bit         |    8-bit        |    8-bit         |
            */

            if (base64Chars != null && base64Chars.Length != 64)
            {
                throw new Exception("There must be 64 chars in base64Chars char array !");
            }

            if (base64Chars == null)
            {
                base64Chars = new char[]{
					'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
					'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
					'0','1','2','3','4','5','6','7','8','9','+','/'
				};
            }

            //--- Create decode table ---------------------//
            byte[] decodeTable = new byte[128];
            for (int i = 0; i < 128; i++)
            {
                int mappingIndex = -1;
                for (int bc = 0; bc < base64Chars.Length; bc++)
                {
                    if (i == base64Chars[bc])
                    {
                        mappingIndex = bc;
                        break;
                    }
                }

                if (mappingIndex > -1)
                {
                    decodeTable[i] = (byte)mappingIndex;
                }
                else
                {
                    decodeTable[i] = 0xFF;
                }
            }
            //---------------------------------------------//

            byte[] decodedDataBuffer = new byte[((base64Data.Length * 6) / 8) + 4];
            int decodedBytesCount = 0;
            int nByteInBase64Block = 0;
            byte[] decodedBlock = new byte[3];
            byte[] base64Block = new byte[4];

            for (int i = 0; i < base64Data.Length; i++)
            {
                byte b = base64Data[i];
                if (b == '=')
                {
                    base64Block[nByteInBase64Block] = 0xFF;
                }
                else
                {
                    byte decodeByte = decodeTable[b & 0x7F];
                    if (decodeByte != 0xFF)
                    {
                        base64Block[nByteInBase64Block] = decodeByte;
                        nByteInBase64Block++;
                    }
                }
                int encodedBytesCount = -1;
                // We have full 4 byte base64 block
                if (nByteInBase64Block == 4)
                {
                    encodedBytesCount = 3;
                }
                // We have reached at the end of base64 data, there may be some bytes left
                else if (i == base64Data.Length - 1)
                {
                    // Invalid value, we can't have only 6 bit, just skip 
                    if (nByteInBase64Block == 1)
                    {
                        encodedBytesCount = 0;
                    }
                    // There is 1 byte in two base64 bytes (6 + 2 bit)
                    else if (nByteInBase64Block == 2)
                    {
                        encodedBytesCount = 1;
                    }
                    // There are 2 bytes in two base64 bytes ([6 + 2],[4 + 4] bit)
                    else if (nByteInBase64Block == 3)
                    {
                        encodedBytesCount = 2;
                    }
                }

                if (encodedBytesCount > -1)
                {
                    decodedDataBuffer[decodedBytesCount + 0] = (byte)((int)base64Block[0] << 2 | (int)base64Block[1] >> 4);
                    decodedDataBuffer[decodedBytesCount + 1] = (byte)(((int)base64Block[1] & 0xF) << 4 | (int)base64Block[2] >> 2);
                    decodedDataBuffer[decodedBytesCount + 2] = (byte)(((int)base64Block[2] & 0x3) << 6 | (int)base64Block[3] >> 0);

                    decodedBytesCount += encodedBytesCount;

                    nByteInBase64Block = 0;
                }
            }
            if (decodedBytesCount > -1)
            {
                byte[] retVal = new byte[decodedBytesCount];
                Array.Copy(decodedDataBuffer, 0, retVal, 0, decodedBytesCount);
                return retVal;
            }
            else
            {
                return new byte[0];
            }
        }

        #endregion


        #region static method ComputeMd5

        public static string ComputeMd5(string text, bool hex)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(text));

            if (hex)
            {
                return ToHex(hash).ToLower();
            }
            else
            {
                return System.Text.Encoding.Default.GetString(hash);
            }
        }

        #endregion

        #region static method IsIoCompletionPortsSupported

        [Obsolete("Use method 'IsSocketAsyncSupported' instead.")]
        public static bool IsIoCompletionPortsSupported()
        {
            try
            {
                using (SocketAsyncEventArgs e = new SocketAsyncEventArgs())
                {
                    return true;
                }
            }
            catch (NotSupportedException nX)
            {
                string dummy = nX.Message;

                return false;
            }
        }

        #endregion

    }
}
