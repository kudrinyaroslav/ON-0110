/*-------------------------------------------------------------------------------------------

Copyright (C) 2009, Open Network Video Interface Forum Inc. (ONVIF), http://www.onvif.org/

-------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Onvif
{
    class JpegHeaderFactory
    {
        #region HuffmanCodeTables

        byte[] lum_dc_codelens = { 0, 1, 5, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 };

        byte[] lum_dc_symbols = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

        byte[] lum_ac_codelens = { 0, 2, 1, 3, 3, 2, 4, 3, 5, 5, 4, 4, 0, 0, 1, 0x7d };

        byte[] lum_ac_symbols = {
        0x01, 0x02, 0x03, 0x00, 0x04, 0x11, 0x05, 0x12,
        0x21, 0x31, 0x41, 0x06, 0x13, 0x51, 0x61, 0x07,
        0x22, 0x71, 0x14, 0x32, 0x81, 0x91, 0xa1, 0x08,
        0x23, 0x42, 0xb1, 0xc1, 0x15, 0x52, 0xd1, 0xf0,
        0x24, 0x33, 0x62, 0x72, 0x82, 0x09, 0x0a, 0x16,
        0x17, 0x18, 0x19, 0x1a, 0x25, 0x26, 0x27, 0x28,
        0x29, 0x2a, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39,
        0x3a, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49,
        0x4a, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59,
        0x5a, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69,
        0x6a, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79,
        0x7a, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89,
        0x8a, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98,
        0x99, 0x9a, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7,
        0xa8, 0xa9, 0xaa, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6,
        0xb7, 0xb8, 0xb9, 0xba, 0xc2, 0xc3, 0xc4, 0xc5,
        0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xd2, 0xd3, 0xd4,
        0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda, 0xe1, 0xe2,
        0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9, 0xea,
        0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8,
        0xf9, 0xfa,
        };

        byte[] chm_dc_codelens = { 0, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 };

        byte[] chm_dc_symbols = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

        byte[] chm_ac_codelens = { 0, 2, 1, 2, 4, 4, 3, 4, 7, 5, 4, 4, 0, 1, 2, 0x77 };

        byte[] chm_ac_symbols = {
        0x00, 0x01, 0x02, 0x03, 0x11, 0x04, 0x05, 0x21,
        0x31, 0x06, 0x12, 0x41, 0x51, 0x07, 0x61, 0x71,
        0x13, 0x22, 0x32, 0x81, 0x08, 0x14, 0x42, 0x91,
        0xa1, 0xb1, 0xc1, 0x09, 0x23, 0x33, 0x52, 0xf0,
        0x15, 0x62, 0x72, 0xd1, 0x0a, 0x16, 0x24, 0x34,
        0xe1, 0x25, 0xf1, 0x17, 0x18, 0x19, 0x1a, 0x26,
        0x27, 0x28, 0x29, 0x2a, 0x35, 0x36, 0x37, 0x38,
        0x39, 0x3a, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48,
        0x49, 0x4a, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58,
        0x59, 0x5a, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68,
        0x69, 0x6a, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78,
        0x79, 0x7a, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87,
        0x88, 0x89, 0x8a, 0x92, 0x93, 0x94, 0x95, 0x96,
        0x97, 0x98, 0x99, 0x9a, 0xa2, 0xa3, 0xa4, 0xa5,
        0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xb2, 0xb3, 0xb4,
        0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xc2, 0xc3,
        0xc4, 0xc5, 0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xd2,
        0xd3, 0xd4, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda,
        0xe2, 0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9,
        0xea, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8,
        0xf9, 0xfa,
        };

        #endregion

        private Dictionary<int, byte[]> _quantTables = new Dictionary<int, byte[]>();

        private int _luminanceVerticalSampling;

        public int LuminanceVerticalSampling
        {
            get { return _luminanceVerticalSampling; }
            set { _luminanceVerticalSampling = value; }
        }

        private int _width;

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        private int _height;

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public void SetQuantTable(int tableNumber, byte[] qTable)
        {
            _quantTables[tableNumber] = qTable;
        }

        public byte[] GetHeaders()
        {
            List<byte> headers = new List<byte>();
            //prepend JM_SOI ffd8
            headers.Add(0xff);
            headers.Add(0xd8);

            //prepend 4 huffman headers
            MakeHuffmanHeader(headers, lum_dc_codelens, lum_dc_symbols, 0, 0);
            MakeHuffmanHeader(headers, lum_ac_codelens, lum_ac_symbols, 0, 1);
            MakeHuffmanHeader(headers, chm_dc_codelens, chm_dc_symbols, 1, 0);
            MakeHuffmanHeader(headers, chm_ac_codelens, chm_ac_symbols, 1, 1);

            //prepend JM_SOF0 ffC0
            headers.Add(0xff);
            headers.Add(0xc0);
            List<byte> SOF = new List<byte>();
            SOF.Add(8);  //8 bit precision
            SOF.Add((byte)(_height >> 8));
            SOF.Add((byte)(_height & 0xFF));
            SOF.Add((byte)(_width >> 8));
            SOF.Add((byte)(_width & 0xFF));
            SOF.Add(3); //number of components
            SOF.Add(1); //luminance ID
            SOF.Add((byte)(_luminanceVerticalSampling == 2 ? 0x22 : 0x21)); //luminance horz & vert sampling
            SOF.Add(0); //luminace quant table
            SOF.Add(2); //chrom ID
            SOF.Add(0x11);//chrom horz & vert sampling
            SOF.Add(1); //chrom quant table
            SOF.Add(3); //chrom ID
            SOF.Add(0x11);//chrom horz & vert sampling
            SOF.Add(1); //chrom quant table
            int sofSize = SOF.Count + 2;
            headers.Add((byte)(sofSize >> 8));
            headers.Add((byte)(sofSize & 0xFF));
            headers.AddRange(SOF);



            //prepend JM_DQT ffdb
            for (int t = 0; t < _quantTables.Count; t++)
            {
                byte[] table = _quantTables[t];
                headers.Add(0xff);
                headers.Add(0xdb);
                int dqtHeaderLength = table.Length + 3;
                headers.Add((byte)(dqtHeaderLength >> 8));
                headers.Add((byte)(dqtHeaderLength & 0xFF));
                headers.Add((byte)t);
                headers.AddRange(table);
            }






            //prepend JM_SOS ffDA
            headers.Add(0xff);
            headers.Add(0xda);
            List<byte> SOS = new List<byte>();
            SOS.Add(3); //number of components
            SOS.Add(1);
            SOS.Add(0x0);
            SOS.Add(2);
            SOS.Add(0x11);
            SOS.Add(3);
            SOS.Add(0x11);
            SOS.Add(0);
            SOS.Add(63);
            SOS.Add(0);
            int sosSize = SOS.Count + 2;
            headers.Add((byte)(sosSize >> 8));
            headers.Add((byte)(sosSize & 0xFF));
            headers.AddRange(SOS);

            return headers.ToArray();
        }

        private void MakeHuffmanHeader(List<byte> destination, byte[] codelens,
                  byte[] symbols, int tableNo, int tableClass)
        {
            destination.Add(0xff);
            destination.Add(0xc4);            /* DHT */
            destination.Add(0);               /* length msb */
            destination.Add((byte)(3 + codelens.Length + symbols.Length)); /* length lsb */
            destination.Add((byte)((tableClass << 4) | tableNo));
            destination.AddRange(codelens);
            destination.AddRange(symbols);
        }

    }
}
