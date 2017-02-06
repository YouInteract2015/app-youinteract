using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You_Contacts.Utils.MIME
{
    public class Mimes
    {
        #region static method DateTimeToRfc2822

        public static string DateTimeToRfc2822(DateTime dateTime)
        {
            return dateTime.ToString("ddd, dd MMM yyyy HH':'mm':'ss ", System.Globalization.DateTimeFormatInfo.InvariantInfo) + dateTime.ToString("zzz").Replace(":", "");
        }

        #endregion

        #region static method ParseRfc2822DateTime

        public static DateTime ParseRfc2822DateTime(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(value);
            }

            /*      date-time       =       [ day-of-week "," ] date FWS time [CFWS]
             *      day-of-week     =       ([FWS] day-name) / obs-day-of-week
             *      day-name        =       "Mon" / "Tue" / "Wed" / "Thu" / "Fri" / "Sat" / "Sun"
             *      date            =       day month year 
             *      year            =       4*DIGIT / obs-year
             *      month           =       (FWS month-name FWS) / obs-month
             *      month-name      =       "Jan" / "Feb" / "Mar" / "Apr" / "May" / "Jun" / "Jul" / "Aug" / "Sep" / "Oct" / "Nov" / "Dec"
             *      day             =       ([FWS] 1*2DIGIT) / obs-day
             *      time            =       time-of-day FWS zone
             *      time-of-day     =       hour ":" minute [ ":" second ]
             *      hour            =       2DIGIT / obs-hour
             *      minute          =       2DIGIT / obs-minute
             *      second          =       2DIGIT / obs-second
             *      zone            =       (( "+" / "-" ) 4DIGIT) / obs-zone
             * 
             *      The date and time-of-day SHOULD express local time.
            */

            try
            {
                MimeReader r = new MimeReader(value);
                string v = r.Atom();
                if (v.Length == 3)
                {
                    r.Char(true);
                    v = r.Atom();
                }
                int day = Convert.ToInt32(v);
                v = r.Atom().ToLower();
                int month = 1;
                if (v == "jan")
                {
                    month = 1;
                }
                else if (v == "feb")
                {
                    month = 2;
                }
                else if (v == "mar")
                {
                    month = 3;
                }
                else if (v == "apr")
                {
                    month = 4;
                }
                else if (v == "may")
                {
                    month = 5;
                }
                else if (v == "jun")
                {
                    month = 6;
                }
                else if (v == "jul")
                {
                    month = 7;
                }
                else if (v == "aug")
                {
                    month = 8;
                }
                else if (v == "sep")
                {
                    month = 9;
                }
                else if (v == "oct")
                {
                    month = 10;
                }
                else if (v == "nov")
                {
                    month = 11;
                }
                else if (v == "dec")
                {
                    month = 12;
                }
                else
                {
                    throw new ArgumentException("Invalid month-name value '" + value + "'.");
                }
                int year = Convert.ToInt32(r.Atom());
                int hour = Convert.ToInt32(r.Atom());
                r.Char(true);
                int minute = Convert.ToInt32(r.Atom());
                int second = 0;
                if (r.Peek(true) == ':')
                {
                    r.Char(true);
                    second = Convert.ToInt32(r.Atom());
                }
                int timeZoneMinutes = 0;
                v = r.Atom();
                // Time zone missing. Not RFC syntax, but some servers will send such dates.
                if (v == null)
                {
                    // Just consider time zone as 0(GMT).
                }
                else if (v[0] == '+' || v[0] == '-')
                {
                    if (v[0] == '+')
                    {
                        timeZoneMinutes = (Convert.ToInt32(v.Substring(1, 2)) * 60 + Convert.ToInt32(v.Substring(3, 2)));
                    }
                    else
                    {
                        timeZoneMinutes = -(Convert.ToInt32(v.Substring(1, 2)) * 60 + Convert.ToInt32(v.Substring(3, 2)));
                    }
                }
                else
                {
                    v = v.ToUpper();

                    #region time zones

                    // Alpha Time Zone (military).
                    if (v == "A")
                    {
                        timeZoneMinutes = ((01 * 60) + 00);
                    }
                    // Australian Central Daylight Time.
                    else if (v == "ACDT")
                    {
                        timeZoneMinutes = ((10 * 60) + 30);
                    }
                    // Australian Central Standard Time.
                    else if (v == "ACST")
                    {
                        timeZoneMinutes = ((09 * 60) + 30);
                    }
                    // Atlantic Daylight Time.
                    else if (v == "ADT")
                    {
                        timeZoneMinutes = -((03 * 60) + 00);
                    }
                    // Australian Eastern Daylight Time.
                    else if (v == "AEDT")
                    {
                        timeZoneMinutes = ((11 * 60) + 00);
                    }
                    // Australian Eastern Standard Time.
                    else if (v == "AEST")
                    {
                        timeZoneMinutes = ((10 * 60) + 00);
                    }
                    // Alaska Daylight Time.
                    else if (v == "AKDT")
                    {
                        timeZoneMinutes = -((08 * 60) + 00);
                    }
                    // Alaska Standard Time.
                    else if (v == "AKST")
                    {
                        timeZoneMinutes = -((09 * 60) + 00);
                    }
                    // Atlantic Standard Time.
                    else if (v == "AST")
                    {
                        timeZoneMinutes = -((04 * 60) + 00);
                    }
                    // Australian Western Daylight Time.
                    else if (v == "AWDT")
                    {
                        timeZoneMinutes = ((09 * 60) + 00);
                    }
                    // Australian Western Standard Time.
                    else if (v == "AWST")
                    {
                        timeZoneMinutes = ((08 * 60) + 00);
                    }
                    // Bravo Time Zone (millitary).
                    else if (v == "B")
                    {
                        timeZoneMinutes = ((02 * 60) + 00);
                    }
                    // British Summer Time.
                    else if (v == "BST")
                    {
                        timeZoneMinutes = ((01 * 60) + 00);
                    }
                    // Charlie Time Zone (millitary).
                    else if (v == "C")
                    {
                        timeZoneMinutes = ((03 * 60) + 00);
                    }
                    // Central Daylight Time.
                    else if (v == "CDT")
                    {
                        timeZoneMinutes = -((05 * 60) + 00);
                    }
                    // Central European Daylight Time.
                    else if (v == "CEDT")
                    {
                        timeZoneMinutes = ((02 * 60) + 00);
                    }
                    // Central European Summer Time.
                    else if (v == "CEST")
                    {
                        timeZoneMinutes = ((02 * 60) + 00);
                    }
                    // Central European Time.
                    else if (v == "CET")
                    {
                        timeZoneMinutes = ((01 * 60) + 00);
                    }
                    // Central Standard Time.
                    else if (v == "CST")
                    {
                        timeZoneMinutes = -((06 * 60) + 00);
                    }
                    // Christmas Island Time.
                    else if (v == "CXT")
                    {
                        timeZoneMinutes = ((01 * 60) + 00);
                    }
                    // Delta Time Zone (military).
                    else if (v == "D")
                    {
                        timeZoneMinutes = ((04 * 60) + 00);
                    }
                    // Echo Time Zone (military).
                    else if (v == "E")
                    {
                        timeZoneMinutes = ((05 * 60) + 00);
                    }
                    // Eastern Daylight Time.
                    else if (v == "EDT")
                    {
                        timeZoneMinutes = -((04 * 60) + 00);
                    }
                    // Eastern European Daylight Time.
                    else if (v == "EEDT")
                    {
                        timeZoneMinutes = ((03 * 60) + 00);
                    }
                    // Eastern European Summer Time.
                    else if (v == "EEST")
                    {
                        timeZoneMinutes = ((03 * 60) + 00);
                    }
                    // Eastern European Time.
                    else if (v == "EET")
                    {
                        timeZoneMinutes = ((02 * 60) + 00);
                    }
                    // Eastern Standard Time.
                    else if (v == "EST")
                    {
                        timeZoneMinutes = -((05 * 60) + 00);
                    }
                    // Foxtrot Time Zone (military).
                    else if (v == "F")
                    {
                        timeZoneMinutes = (06 * 60 + 00);
                    }
                    // Golf Time Zone (military).
                    else if (v == "G")
                    {
                        timeZoneMinutes = ((07 * 60) + 00);
                    }
                    // Greenwich Mean Time.
                    else if (v == "GMT")
                    {
                        timeZoneMinutes = 0000;
                    }
                    // Hotel Time Zone (military).
                    else if (v == "H")
                    {
                        timeZoneMinutes = ((08 * 60) + 00);
                    }
                    // India Time Zone (military).
                    else if (v == "I")
                    {
                        timeZoneMinutes = ((09 * 60) + 00);
                    }
                    // Irish Summer Time.
                    else if (v == "IST")
                    {
                        timeZoneMinutes = ((01 * 60) + 00);
                    }
                    // Kilo Time Zone (millitary).
                    else if (v == "K")
                    {
                        timeZoneMinutes = ((10 * 60) + 00);
                    }
                    // Lima Time Zone (millitary).
                    else if (v == "L")
                    {
                        timeZoneMinutes = ((11 * 60) + 00);
                    }
                    // Mike Time Zone (millitary).
                    else if (v == "M")
                    {
                        timeZoneMinutes = ((12 * 60) + 00);
                    }
                    // Mountain Daylight Time.
                    else if (v == "MDT")
                    {
                        timeZoneMinutes = -((06 * 60) + 00);
                    }
                    // Mountain Standard Time.
                    else if (v == "MST")
                    {
                        timeZoneMinutes = -((07 * 60) + 00);
                    }
                    // November Time Zone (military).
                    else if (v == "N")
                    {
                        timeZoneMinutes = -((01 * 60) + 00);
                    }
                    // Newfoundland Daylight Time.
                    else if (v == "NDT")
                    {
                        timeZoneMinutes = -((02 * 60) + 30);
                    }
                    // Norfolk (Island) Time.
                    else if (v == "NFT")
                    {
                        timeZoneMinutes = ((11 * 60) + 30);
                    }
                    // Newfoundland Standard Time.
                    else if (v == "NST")
                    {
                        timeZoneMinutes = -((03 * 60) + 30);
                    }
                    // Oscar Time Zone (military).
                    else if (v == "O")
                    {
                        timeZoneMinutes = -((02 * 60) + 00);
                    }
                    // Papa Time Zone (military).
                    else if (v == "P")
                    {
                        timeZoneMinutes = -((03 * 60) + 00);
                    }
                    // Pacific Daylight Time.
                    else if (v == "PDT")
                    {
                        timeZoneMinutes = -((07 * 60) + 00);
                    }
                    // Pacific Standard Time.
                    else if (v == "PST")
                    {
                        timeZoneMinutes = -((08 * 60) + 00);
                    }
                    // Quebec Time Zone (military).
                    else if (v == "Q")
                    {
                        timeZoneMinutes = -((04 * 60) + 00);
                    }
                    // Romeo Time Zone (military).
                    else if (v == "R")
                    {
                        timeZoneMinutes = -((05 * 60) + 00);
                    }
                    // Sierra Time Zone (military).
                    else if (v == "S")
                    {
                        timeZoneMinutes = -((06 * 60) + 00);
                    }
                    // Tango Time Zone (military).
                    else if (v == "T")
                    {
                        timeZoneMinutes = -((07 * 60) + 00);
                    }
                    // Uniform Time Zone (military).
                    else if (v == "")
                    {
                        timeZoneMinutes = -((08 * 60) + 00);
                    }
                    // Coordinated Universal Time.
                    else if (v == "UTC")
                    {
                        timeZoneMinutes = 0000;
                    }
                    // Victor Time Zone (militray).
                    else if (v == "V")
                    {
                        timeZoneMinutes = -((09 * 60) + 00);
                    }
                    // Whiskey Time Zone (military).
                    else if (v == "W")
                    {
                        timeZoneMinutes = -((10 * 60) + 00);
                    }
                    // Western European Daylight Time.
                    else if (v == "WEDT")
                    {
                        timeZoneMinutes = ((01 * 60) + 00);
                    }
                    // Western European Summer Time.
                    else if (v == "WEST")
                    {
                        timeZoneMinutes = ((01 * 60) + 00);
                    }
                    // Western European Time.
                    else if (v == "WET")
                    {
                        timeZoneMinutes = 0000;
                    }
                    // Western Standard Time.
                    else if (v == "WST")
                    {
                        timeZoneMinutes = ((08 * 60) + 00);
                    }
                    // X-ray Time Zone (military).
                    else if (v == "X")
                    {
                        timeZoneMinutes = -((11 * 60) + 00);
                    }
                    // Yankee Time Zone (military).
                    else if (v == "Y")
                    {
                        timeZoneMinutes = -((12 * 60) + 00);
                    }
                    // Zulu Time Zone (military).
                    else if (v == "Z")
                    {
                        timeZoneMinutes = 0000;
                    }

                    #endregion
                }

                // Convert time to UTC and then back to local.
                DateTime timeUTC = new DateTime(year, month, day, hour, minute, second).AddMinutes(-(timeZoneMinutes));
                return new DateTime(timeUTC.Year, timeUTC.Month, timeUTC.Day, timeUTC.Hour, timeUTC.Minute, timeUTC.Second, DateTimeKind.Utc).ToLocalTime();
            }
            catch (Exception x)
            {
                string dymmy = x.Message;
                throw new ArgumentException("Argumnet 'value' value '" + value + "' is not valid RFC 822/2822 date-time string.");
            }
        }

        #endregion


        #region static method UnfoldHeader

        public static string UnfoldHeader(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            return value.Replace("\r\n", "");
        }

        #endregion


        #region static method CreateMessageID

        public static string CreateMessageID()
        {
            return "<" + Guid.NewGuid().ToString().Replace("-", "").Substring(16) + "@" + Guid.NewGuid().ToString().Replace("-", "").Substring(16) + ">";
        }

        #endregion


        #region static method ParseHeaders

        internal static string ParseHeaders(Stream entryStrm)
        {
            byte[] crlf = new byte[] { (byte)'\r', (byte)'\n' };
            MemoryStream msHeaders = new MemoryStream();
            StreamReader r = new StreamReader(entryStrm);
            byte[] lineData = Encoding.UTF8.GetBytes(r.ReadLine());
            while (lineData != null)
            {
                if (lineData.Length == 0)
                {
                    break;
                }

                msHeaders.Write(lineData, 0, lineData.Length);
                msHeaders.Write(crlf, 0, crlf.Length);
                lineData = Encoding.UTF8.GetBytes(r.ReadLine());
            }

            return System.Text.Encoding.Default.GetString(msHeaders.ToArray());
        }

        #endregion

        #region static method ParseHeaderField

        public static string ParseHeaderField(string fieldName, Stream entryStrm)
        {
            return ParseHeaderField(fieldName, ParseHeaders(entryStrm));
        }

        public static string ParseHeaderField(string fieldName, string headers)
        {
            using (TextReader r = new StreamReader(new MemoryStream(System.Text.Encoding.Default.GetBytes(headers))))
            {
                string line = r.ReadLine();
                while (line != null)
                {

                    if (line.ToUpper().StartsWith(fieldName.ToUpper()))
                    {

                        string fieldValue = line.Substring(fieldName.Length).Trim();

                        line = r.ReadLine();
                        while (line != null && (line.StartsWith("\t") || line.StartsWith(" ")))
                        {
                            fieldValue += line;
                            line = r.ReadLine();
                        }

                        return fieldValue;
                    }

                    line = r.ReadLine();
                }
            }

            return "";
        }

        #endregion


        #region static method QDecode

        public static string QDecode(Encoding encoding, string data)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            return encoding.GetString(QuotedPrintableDecode(Encoding.ASCII.GetBytes(data.Replace("_", " "))));
        }

        #endregion

        #region static method QuotedPrintableDecode

        public static byte[] QuotedPrintableDecode(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            MemoryStream msRetVal = new MemoryStream();
            MemoryStream msSourceStream = new MemoryStream(data);

            int b = msSourceStream.ReadByte();
            while (b > -1)
            {
                if (b == '=')
                {
                    byte[] buffer = new byte[2];
                    int nCount = msSourceStream.Read(buffer, 0, 2);
                    if (nCount == 2)
                    {
                        if (buffer[0] == '\r' && buffer[1] == '\n')
                        {
                        }
                        else
                        {
                            try
                            {
                                msRetVal.Write(Net.FromHex(buffer), 0, 1);
                            }
                            catch
                            {
                                msRetVal.WriteByte((byte)'=');
                                msRetVal.Write(buffer, 0, 2);
                            }
                        }
                    }
                    else
                    {
                        msRetVal.Write(buffer, 0, nCount);
                    }
                }
                else
                {
                    msRetVal.WriteByte((byte)b);
                }

                b = msSourceStream.ReadByte();
            }

            return msRetVal.ToArray();
        }

        #endregion
    }
}
