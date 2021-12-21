// Decompiled with JetBrains decompiler
// Type: WumbasWigwam.MidiParse
// Assembly: WumbasWigwam, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E1B20CF-FC55-4FDF-8F94-7BCA06D01AA5
// Assembly location: C:\Users\Spice\Desktop\RareHacking\WumbasWigwam\WumbasWigwam.exe

using System;
using System.IO;
using System.Windows.Forms;

namespace WumbasWigwam
{
  public static class MidiParse
  {
    private static uint CharArrayToLong(byte[] currentSpot, int offset) => Convert.ToUInt32((uint) ((((int) currentSpot[offset] << 8 | (int) currentSpot[offset + 1]) << 8 | (int) currentSpot[offset + 2]) << 8) | (uint) currentSpot[offset + 3]);

    private static ushort CharArrayToShort(byte[] currentSpot, int offset) => Convert.ToUInt16((int) currentSpot[offset] << 8 | (int) currentSpot[offset + 1]);

    private static uint Flip32Bit(uint inLong) => Convert.ToUInt32((uint) ((int) ((inLong & 4278190080U) >> 24) | (int) ((inLong & 16711680U) >> 8) | ((int) inLong & 65280) << 8 | ((int) inLong & (int) byte.MaxValue) << 24));

    private static ushort Flip16Bit(ushort tempShort) => Convert.ToUInt16(((int) tempShort & 65280) >> 8 | ((int) tempShort & (int) byte.MaxValue) << 8);

    private static uint GetVLBytes(
      byte[] vlByteArray,
      ref int offset,
      ref uint original,
      ref byte[] altPattern,
      ref byte altOffset,
      ref byte altLength,
      bool includeFERepeats)
    {
      uint num1 = 0;
      byte vlByte1;
      while (true)
      {
        if (altPattern != null)
        {
          vlByte1 = altPattern[(int) altOffset];
          ++altOffset;
          if ((int) altOffset == (int) altLength && altPattern != null)
          {
            altPattern = (byte[]) null;
            altOffset = (byte) 0;
            altLength = (byte) 0;
          }
        }
        else
        {
          vlByte1 = vlByteArray[offset];
          ++offset;
          if (((vlByte1 != (byte) 254 ? 0 : (vlByteArray[offset] != (byte) 254 ? 1 : 0)) & (includeFERepeats ? 1 : 0)) != 0)
          {
            int vlByte2 = (int) vlByteArray[offset];
            ++offset;
            ushort uint16 = Convert.ToUInt16(vlByte2 << 8 | (int) vlByteArray[offset]);
            ++offset;
            byte vlByte3 = vlByteArray[offset];
            ++offset;
            altPattern = new byte[(int) vlByte3];
            for (int index = offset - 4 - (int) uint16; index < offset - 4 - (int) uint16 + (int) vlByte3; ++index)
              altPattern[index - (offset - 4 - (int) uint16)] = vlByteArray[index];
            altOffset = (byte) 0;
            altLength = vlByte3;
            vlByte1 = altPattern[(int) altOffset];
            ++altOffset;
          }
          else if (((vlByte1 != (byte) 254 ? 0 : (vlByteArray[offset] == (byte) 254 ? 1 : 0)) & (includeFERepeats ? 1 : 0)) != 0)
            ++offset;
          if ((int) altOffset == (int) altLength && altPattern != null)
          {
            altPattern = (byte[]) null;
            altOffset = (byte) 0;
            altLength = (byte) 0;
          }
        }
        if ((int) vlByte1 >> 7 == 1)
          num1 = num1 + (uint) vlByte1 << 8;
        else
          break;
      }
      uint num2 = num1 + (uint) vlByte1;
      original = num2;
      uint num3 = 0;
      int num4 = 0;
      int num5 = 0;
      while (true)
      {
        num3 += (uint) (((int) (num2 >> num4) & (int) sbyte.MaxValue) << num5);
        if (num4 != 24)
        {
          num4 += 8;
          num5 += 7;
        }
        else
          break;
      }
      return num3;
    }

    private static void WriteVLBytes(
      FileStream outFile,
      uint value,
      uint length,
      bool includeFERepeats)
    {
      switch (length)
      {
        case 1:
          byte num1 = (byte) (value & (uint) byte.MaxValue);
          outFile.WriteByte(num1);
          break;
        case 2:
          byte num2 = (byte) (value >> 8 & (uint) byte.MaxValue);
          outFile.WriteByte(num2);
          byte num3 = (byte) (value & (uint) byte.MaxValue);
          outFile.WriteByte(num3);
          break;
        case 3:
          byte num4 = (byte) (value >> 16 & (uint) byte.MaxValue);
          outFile.WriteByte(num4);
          byte num5 = (byte) (value >> 8 & (uint) byte.MaxValue);
          outFile.WriteByte(num5);
          byte num6 = (byte) (value & (uint) byte.MaxValue);
          outFile.WriteByte(num6);
          break;
        default:
          byte num7 = (byte) (value >> 24 & (uint) byte.MaxValue);
          outFile.WriteByte(num7);
          byte num8 = (byte) (value >> 8 & (uint) byte.MaxValue);
          outFile.WriteByte(num8);
          byte num9 = (byte) (value & (uint) byte.MaxValue);
          outFile.WriteByte(num9);
          break;
      }
    }

    private static byte ReadMidiByte(
      byte[] vlByteArray,
      ref int offset,
      ref byte[] altPattern,
      ref byte altOffset,
      ref byte altLength,
      bool includeFERepeats)
    {
      byte vlByte1;
      if (altPattern != null)
      {
        vlByte1 = altPattern[(int) altOffset];
        ++altOffset;
      }
      else
      {
        vlByte1 = vlByteArray[offset];
        ++offset;
        if (((vlByte1 != (byte) 254 ? 0 : (vlByteArray[offset] != (byte) 254 ? 1 : 0)) & (includeFERepeats ? 1 : 0)) != 0)
        {
          int vlByte2 = (int) vlByteArray[offset];
          ++offset;
          uint num = (uint) (vlByte2 << 8) | (uint) vlByteArray[offset];
          ++offset;
          byte vlByte3 = vlByteArray[offset];
          ++offset;
          altPattern = new byte[(int) vlByte3];
          for (int int32 = Convert.ToInt32((long) (offset - 4) - (long) num); (long) int32 < (long) (offset - 4) - (long) num + (long) vlByte3; ++int32)
            altPattern[(long) int32 - ((long) (offset - 4) - (long) num)] = vlByteArray[int32];
          altOffset = (byte) 0;
          altLength = vlByte3;
          vlByte1 = altPattern[(int) altOffset];
          ++altOffset;
        }
        else if (((vlByte1 != (byte) 254 ? 0 : (vlByteArray[offset] == (byte) 254 ? 1 : 0)) & (includeFERepeats ? 1 : 0)) != 0)
          ++offset;
      }
      if ((int) altOffset == (int) altLength && altPattern != null)
      {
        altPattern = (byte[]) null;
        altOffset = (byte) 0;
        altLength = (byte) 0;
      }
      return vlByte1;
    }

    private static uint ReturnVLBytes(uint value, ref uint length)
    {
      byte num1 = Convert.ToByte(value >> 21 & (uint) sbyte.MaxValue);
      byte num2 = Convert.ToByte(value >> 14 & (uint) sbyte.MaxValue);
      byte num3 = Convert.ToByte(value >> 7 & (uint) sbyte.MaxValue);
      byte num4 = Convert.ToByte(value & (uint) sbyte.MaxValue);
      if (num1 > (byte) 0)
      {
        int num5 = -2139062272 | (int) num1 << 24 | (int) num2 << 16 | (int) num3 << 8 | (int) num4;
        length = 4U;
        return (uint) num5;
      }
      if (num2 > (byte) 0)
      {
        int num6 = 8421376 | (int) num2 << 16 | (int) num3 << 8 | (int) num4;
        length = 3U;
        return (uint) num6;
      }
      if (num3 > (byte) 0)
      {
        int num7 = 32768 | (int) num3 << 8 | (int) num4;
        length = 2U;
        return (uint) num7;
      }
      length = 1U;
      return value;
    }

    private static void WriteLongToBuffer(byte[] Buffer, uint address, uint data)
    {
      Buffer[(int) address] = Convert.ToByte(data >> 24 & (uint) byte.MaxValue);
      Buffer[(int) address + 1] = Convert.ToByte(data >> 16 & (uint) byte.MaxValue);
      Buffer[(int) address + 2] = Convert.ToByte(data >> 8 & (uint) byte.MaxValue);
      Buffer[(int) address + 3] = Convert.ToByte(data & (uint) byte.MaxValue);
    }

    public static void GEMidiToMidi(
      byte[] inputMID,
      int inputSize,
      string outFileName,
      ref int numberInstruments)
    {
      numberInstruments = 0;
      try
      {
        FileStream outFile = new FileStream(outFileName, FileMode.Create, FileAccess.Write);
        if (outFile == null)
        {
          int num1 = (int) MessageBox.Show("Error outputting file", "Error");
        }
        else
        {
          uint num2 = 68;
          int num3 = 0;
          for (int offset = 0; (long) offset < (long) (num2 - 4U); offset += 4)
          {
            if (MidiParse.CharArrayToLong(inputMID, offset) != 0U)
              ++num3;
          }
          uint num4 = MidiParse.Flip32Bit(1297377380U);
          outFile.Write(BitConverter.GetBytes(num4), 0, 4);
          uint num5 = MidiParse.Flip32Bit(6U);
          outFile.Write(BitConverter.GetBytes(num5), 0, 4);
          uint num6 = MidiParse.Flip32Bit(Convert.ToUInt32(65536 | num3));
          outFile.Write(BitConverter.GetBytes(num6), 0, 4);
          ushort num7 = MidiParse.Flip16Bit(Convert.ToUInt16(MidiParse.CharArrayToLong(inputMID, 64)));
          outFile.Write(BitConverter.GetBytes(num7), 0, 2);
          int num8 = 0;
          for (int offset = 0; (long) offset < (long) (num2 - 4U); offset += 4)
          {
            uint num9 = 0;
            int index1 = 0;
            TrackEvent[] trackEventArray = new TrackEvent[1048576];
            for (int index2 = 0; index2 < 1048576; ++index2)
            {
              trackEventArray[index2] = new TrackEvent();
              trackEventArray[index2].contents = (byte[]) null;
              trackEventArray[index2].contentSize = 0;
              trackEventArray[index2].obsoleteEvent = false;
              trackEventArray[index2].deltaTime = 0U;
              trackEventArray[index2].absoluteTime = 0U;
            }
            int int32 = Convert.ToInt32(MidiParse.CharArrayToLong(inputMID, offset));
            if (int32 != 0)
            {
              uint num10 = MidiParse.Flip32Bit(1297379947U);
              outFile.Write(BitConverter.GetBytes(num10), 0, 4);
              int num11 = 0;
              byte[] altPattern = (byte[]) null;
              byte altOffset = 0;
              byte altLength = 0;
              bool flag1 = false;
              while (int32 < inputSize && !flag1)
              {
                if (index1 > 589824)
                  return;
                uint original = 0;
                uint vlBytes1 = MidiParse.GetVLBytes(inputMID, ref int32, ref original, ref altPattern, ref altOffset, ref altLength, true);
                trackEventArray[index1].deltaTime += vlBytes1;
                num9 += vlBytes1;
                trackEventArray[index1].absoluteTime = num9;
                byte num12 = MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, true);
                bool flag2 = num12 < (byte) 128;
                if (num12 == byte.MaxValue || flag2 && num11 == (int) byte.MaxValue)
                {
                  switch (!flag2 ? MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, true) : num12)
                  {
                    case 45:
                      int num13 = (int) MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, true);
                      int num14 = (int) MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, true);
                      int num15 = (int) MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, false);
                      int num16 = (int) MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, false);
                      int num17 = (int) MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, false);
                      int num18 = (int) MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, false);
                      break;
                    case 46:
                      int num19 = (int) MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, true);
                      if (MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, true) == byte.MaxValue)
                        break;
                      break;
                    case 47:
                      trackEventArray[index1].type = byte.MaxValue;
                      trackEventArray[index1].contentSize = 2;
                      trackEventArray[index1].contents = new byte[trackEventArray[index1].contentSize];
                      trackEventArray[index1].contents[0] = (byte) 47;
                      trackEventArray[index1].contents[1] = (byte) 0;
                      ++index1;
                      flag1 = true;
                      break;
                    case 81:
                      int num20 = ((int) MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, true) << 8 | (int) MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, true)) << 8 | (int) MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, true);
                      trackEventArray[index1].type = byte.MaxValue;
                      trackEventArray[index1].contentSize = 5;
                      trackEventArray[index1].contents = new byte[trackEventArray[index1].contentSize];
                      trackEventArray[index1].contents[0] = (byte) 81;
                      trackEventArray[index1].contents[1] = (byte) 3;
                      trackEventArray[index1].contents[2] = Convert.ToByte(num20 >> 16 & (int) byte.MaxValue);
                      trackEventArray[index1].contents[3] = Convert.ToByte(num20 >> 8 & (int) byte.MaxValue);
                      trackEventArray[index1].contents[4] = Convert.ToByte(num20 & (int) byte.MaxValue);
                      ++index1;
                      double num21 = 60000000.0 / (double) num20;
                      break;
                  }
                  if (!flag2)
                    num11 = (int) num12;
                }
                else if (num12 >= (byte) 144 && num12 < (byte) 160 || flag2 && num11 >= 144 && num11 < 160)
                {
                  byte num22;
                  if (flag2)
                  {
                    trackEventArray[index1].type = Convert.ToByte(num11);
                    num22 = num12;
                    int num23 = (int) Convert.ToByte(num11);
                  }
                  else
                  {
                    trackEventArray[index1].type = num12;
                    num22 = MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, true);
                  }
                  byte num24 = MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, true);
                  uint vlBytes2 = MidiParse.GetVLBytes(inputMID, ref int32, ref original, ref altPattern, ref altOffset, ref altLength, true);
                  trackEventArray[index1].durationTime = vlBytes2;
                  trackEventArray[index1].contentSize = 2;
                  trackEventArray[index1].contents = new byte[trackEventArray[index1].contentSize];
                  trackEventArray[index1].contents[0] = num22;
                  trackEventArray[index1].contents[1] = num24;
                  ++index1;
                  if (!flag2)
                    num11 = (int) num12;
                }
                else if (num12 >= (byte) 176 && num12 < (byte) 192 || flag2 && num11 >= 176 && num11 < 192)
                {
                  byte num25;
                  if (flag2)
                  {
                    num25 = num12;
                    trackEventArray[index1].type = Convert.ToByte(num11);
                  }
                  else
                  {
                    num25 = MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, true);
                    trackEventArray[index1].type = num12;
                  }
                  byte num26 = MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, true);
                  trackEventArray[index1].contentSize = 2;
                  trackEventArray[index1].contents = new byte[trackEventArray[index1].contentSize];
                  trackEventArray[index1].contents[0] = num25;
                  trackEventArray[index1].contents[1] = num26;
                  ++index1;
                  if (!flag2)
                    num11 = (int) num12;
                }
                else if (num12 >= (byte) 192 && num12 < (byte) 208 || flag2 && num11 >= 192 && num11 < 208)
                {
                  byte num27;
                  if (flag2)
                  {
                    num27 = num12;
                    trackEventArray[index1].type = Convert.ToByte(num11);
                  }
                  else
                  {
                    num27 = MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, true);
                    trackEventArray[index1].type = num12;
                  }
                  trackEventArray[index1].contentSize = 1;
                  trackEventArray[index1].contents = new byte[trackEventArray[index1].contentSize];
                  trackEventArray[index1].contents[0] = num27;
                  if ((int) num27 >= numberInstruments)
                    numberInstruments = (int) num27 + 1;
                  ++index1;
                  if (!flag2)
                    num11 = (int) num12;
                }
                else if (num12 >= (byte) 208 && num12 < (byte) 224 || flag2 && num11 >= 208 && num11 < 224)
                {
                  byte num28;
                  if (flag2)
                  {
                    num28 = num12;
                    trackEventArray[index1].type = Convert.ToByte(num11);
                  }
                  else
                  {
                    num28 = MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, true);
                    trackEventArray[index1].type = num12;
                  }
                  trackEventArray[index1].contentSize = 1;
                  trackEventArray[index1].contents = new byte[trackEventArray[index1].contentSize];
                  trackEventArray[index1].contents[0] = num28;
                  ++index1;
                  if (!flag2)
                    num11 = (int) num12;
                }
                else if (num12 >= (byte) 224 && num12 < (byte) 240 || flag2 && num11 >= 224 && num11 < 240)
                {
                  byte num29;
                  if (flag2)
                  {
                    num29 = num12;
                    trackEventArray[index1].type = Convert.ToByte(num11);
                  }
                  else
                  {
                    num29 = MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, true);
                    trackEventArray[index1].type = num12;
                  }
                  byte num30 = MidiParse.ReadMidiByte(inputMID, ref int32, ref altPattern, ref altOffset, ref altLength, true);
                  trackEventArray[index1].contentSize = 2;
                  trackEventArray[index1].contents = new byte[trackEventArray[index1].contentSize];
                  trackEventArray[index1].contents[0] = num29;
                  trackEventArray[index1].contents[1] = num30;
                  ++index1;
                  if (!flag2)
                    num11 = (int) num12;
                }
              }
              for (int index3 = 0; index3 < index1; ++index3)
              {
                if (index1 > 589824)
                  return;
                TrackEvent trackEvent = trackEventArray[index3];
                if (trackEvent.type >= (byte) 144 && trackEvent.type < (byte) 160 && trackEvent.durationTime > 0U)
                {
                  uint num31 = trackEvent.absoluteTime + trackEvent.durationTime;
                  if (index3 != index1 - 1)
                  {
                    for (int index4 = index3 + 1; index4 < index1; ++index4)
                    {
                      if (trackEventArray[index4].absoluteTime > num31 && index4 != index1 - 1)
                      {
                        for (int index5 = index1 - 1; index5 >= index4; --index5)
                        {
                          trackEventArray[index5 + 1].absoluteTime = trackEventArray[index5].absoluteTime;
                          trackEventArray[index5 + 1].contentSize = trackEventArray[index5].contentSize;
                          if (trackEventArray[index5 + 1].contents != null)
                            trackEventArray[index5 + 1].contents = (byte[]) null;
                          trackEventArray[index5 + 1].contents = new byte[trackEventArray[index5].contentSize];
                          for (int index6 = 0; index6 < trackEventArray[index5].contentSize; ++index6)
                            trackEventArray[index5 + 1].contents[index6] = trackEventArray[index5].contents[index6];
                          trackEventArray[index5 + 1].deltaTime = trackEventArray[index5].deltaTime;
                          trackEventArray[index5 + 1].durationTime = trackEventArray[index5].durationTime;
                          trackEventArray[index5 + 1].obsoleteEvent = trackEventArray[index5].obsoleteEvent;
                          trackEventArray[index5 + 1].type = trackEventArray[index5].type;
                        }
                        trackEventArray[index4].type = trackEventArray[index3].type;
                        trackEventArray[index4].absoluteTime = num31;
                        trackEventArray[index4].deltaTime = trackEventArray[index4].absoluteTime - trackEventArray[index4 - 1].absoluteTime;
                        trackEventArray[index4].contentSize = trackEventArray[index3].contentSize;
                        trackEventArray[index4].durationTime = 0U;
                        trackEventArray[index4].contents = new byte[trackEventArray[index4].contentSize];
                        trackEventArray[index4].contents[0] = trackEventArray[index3].contents[0];
                        trackEventArray[index4].contents[1] = (byte) 0;
                        trackEventArray[index4 + 1].deltaTime = trackEventArray[index4 + 1].absoluteTime - trackEventArray[index4].absoluteTime;
                        int deltaTime = (int) trackEventArray[index4].deltaTime;
                        ++index1;
                        break;
                      }
                      if (index4 == index1 - 1)
                      {
                        trackEventArray[index4 + 1].absoluteTime = num31;
                        trackEventArray[index4 + 1].contentSize = trackEventArray[index4].contentSize;
                        if (trackEventArray[index4 + 1].contents != null)
                          trackEventArray[index4 + 1].contents = (byte[]) null;
                        trackEventArray[index4 + 1].contents = new byte[trackEventArray[index4].contentSize];
                        for (int index7 = 0; index7 < trackEventArray[index4].contentSize; ++index7)
                          trackEventArray[index4 + 1].contents[index7] = trackEventArray[index4].contents[index7];
                        trackEventArray[index4 + 1].deltaTime = trackEventArray[index4].deltaTime;
                        trackEventArray[index4 + 1].durationTime = trackEventArray[index4].durationTime;
                        trackEventArray[index4 + 1].obsoleteEvent = trackEventArray[index4].obsoleteEvent;
                        trackEventArray[index4 + 1].type = trackEventArray[index4].type;
                        trackEventArray[index4].type = trackEventArray[index3].type;
                        trackEventArray[index4].absoluteTime = num31;
                        trackEventArray[index4].deltaTime = trackEventArray[index4].absoluteTime - trackEventArray[index4 - 1].absoluteTime;
                        trackEventArray[index4].contentSize = trackEventArray[index3].contentSize;
                        trackEventArray[index4].durationTime = 0U;
                        trackEventArray[index4].contents = new byte[trackEventArray[index4].contentSize];
                        trackEventArray[index4].contents[0] = trackEventArray[index3].contents[0];
                        trackEventArray[index4].contents[1] = (byte) 0;
                        trackEventArray[index4 + 1].deltaTime = trackEventArray[index4 + 1].absoluteTime - trackEventArray[index4].absoluteTime;
                        ++index1;
                        break;
                      }
                    }
                  }
                  else
                  {
                    trackEventArray[index3 + 1].absoluteTime = num31;
                    trackEventArray[index3 + 1].contentSize = trackEventArray[index3].contentSize;
                    if (trackEventArray[index3 + 1].contents != null)
                      trackEventArray[index3 + 1].contents = (byte[]) null;
                    trackEventArray[index3 + 1].contents = new byte[trackEventArray[index3].contentSize];
                    for (int index8 = 0; index8 < trackEventArray[index3].contentSize; ++index8)
                      trackEventArray[index3 + 1].contents[index8] = trackEventArray[index3].contents[index8];
                    trackEventArray[index3 + 1].deltaTime = trackEventArray[index3].deltaTime;
                    trackEventArray[index3 + 1].durationTime = trackEventArray[index3].durationTime;
                    trackEventArray[index3 + 1].obsoleteEvent = trackEventArray[index3].obsoleteEvent;
                    trackEventArray[index3 + 1].type = trackEventArray[index3].type;
                    trackEventArray[index3].type = trackEventArray[index3].type;
                    trackEventArray[index3].absoluteTime = num31;
                    int num32 = (int) trackEventArray[index3].absoluteTime - (int) trackEventArray[index3 - 1].absoluteTime;
                    trackEventArray[index3].deltaTime = trackEventArray[index3].absoluteTime - trackEventArray[index3 - 1].absoluteTime;
                    trackEventArray[index3].contentSize = trackEventArray[index3].contentSize;
                    trackEventArray[index3].durationTime = 0U;
                    trackEventArray[index3].contents = new byte[trackEventArray[index3].contentSize];
                    trackEventArray[index3].contents[0] = trackEventArray[index3].contents[0];
                    trackEventArray[index3].contents[1] = (byte) 0;
                    trackEventArray[index3 + 1].deltaTime = trackEventArray[index3 + 1].absoluteTime - trackEventArray[index3].absoluteTime;
                    int deltaTime = (int) trackEventArray[index3].deltaTime;
                    ++index1;
                  }
                }
              }
              uint num33 = 0;
              uint inLong = 0;
              byte num34 = 0;
              for (int index9 = 0; index9 < index1; ++index9)
              {
                TrackEvent trackEvent = trackEventArray[index9];
                if (trackEvent.obsoleteEvent)
                {
                  num33 += trackEvent.deltaTime;
                }
                else
                {
                  uint length = 0;
                  int num35 = (int) MidiParse.ReturnVLBytes(trackEvent.deltaTime + num33, ref length);
                  num33 = 0U;
                  uint num36 = inLong + length;
                  if ((int) trackEvent.type != (int) num34 || trackEvent.type == byte.MaxValue)
                    ++num36;
                  inLong = num36 + Convert.ToUInt32(trackEvent.contentSize);
                  num34 = trackEvent.type;
                }
              }
              uint num37 = MidiParse.Flip32Bit(inLong);
              outFile.Write(BitConverter.GetBytes(num37), 0, 4);
              uint num38 = 0;
              byte num39 = 0;
              for (int index10 = 0; index10 < index1; ++index10)
              {
                TrackEvent trackEvent = trackEventArray[index10];
                if (trackEvent.obsoleteEvent)
                {
                  num38 += trackEvent.deltaTime;
                }
                else
                {
                  uint length = 0;
                  uint num40 = MidiParse.ReturnVLBytes(trackEvent.deltaTime + num38, ref length);
                  num38 = 0U;
                  MidiParse.WriteVLBytes(outFile, num40, length, true);
                  if ((int) trackEvent.type != (int) num39 || trackEvent.type == byte.MaxValue)
                    outFile.WriteByte(trackEvent.type);
                  outFile.Write(trackEvent.contents, 0, trackEvent.contentSize);
                  num39 = trackEvent.type;
                }
              }
              for (int index11 = 0; index11 < index1; ++index11)
              {
                if (trackEventArray[index11].contents != null)
                  trackEventArray[index11].contents = (byte[]) null;
              }
            }
            ++num8;
          }
          outFile.Close();
          outFile.Dispose();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error exporting " + ex.ToString(), "Error");
      }
    }

    public static void BTMidiToMidi(
      byte[] inputMID,
      int inputSize,
      string outFileName,
      ref int numberInstruments)
    {
      numberInstruments = 0;
      try
      {
        FileStream outFile = new FileStream(outFileName, FileMode.Create, FileAccess.Write);
        if (outFile == null)
        {
          int num1 = (int) MessageBox.Show("Error outputting file", "Error");
        }
        else
        {
          int int32_1 = Convert.ToInt32(MidiParse.CharArrayToLong(inputMID, 4));
          uint num2 = MidiParse.Flip32Bit(1297377380U);
          outFile.Write(BitConverter.GetBytes(num2), 0, 4);
          uint num3 = MidiParse.Flip32Bit(6U);
          outFile.Write(BitConverter.GetBytes(num3), 0, 4);
          uint num4 = MidiParse.Flip32Bit(Convert.ToUInt32(65536 | int32_1));
          outFile.Write(BitConverter.GetBytes(num4), 0, 4);
          ushort num5 = MidiParse.Flip16Bit(Convert.ToUInt16(MidiParse.CharArrayToLong(inputMID, 0)));
          outFile.Write(BitConverter.GetBytes(num5), 0, 2);
          int num6 = 0;
          for (int index1 = 0; index1 < int32_1; ++index1)
          {
            uint num7 = 0;
            int index2 = 0;
            TrackEvent[] trackEventArray = new TrackEvent[1048576];
            for (int index3 = 0; index3 < 1048576; ++index3)
            {
              trackEventArray[index3] = new TrackEvent();
              trackEventArray[index3].contents = (byte[]) null;
              trackEventArray[index3].contentSize = 0;
              trackEventArray[index3].obsoleteEvent = false;
              trackEventArray[index3].deltaTime = 0U;
              trackEventArray[index3].absoluteTime = 0U;
            }
            int int32_2 = Convert.ToInt32(MidiParse.CharArrayToLong(inputMID, index1 * 4 + 8));
            if (int32_2 != 0)
            {
              uint num8 = MidiParse.Flip32Bit(1297379947U);
              outFile.Write(BitConverter.GetBytes(num8), 0, 4);
              int num9 = 0;
              byte[] altPattern = (byte[]) null;
              byte altOffset = 0;
              byte altLength = 0;
              bool flag1 = false;
              while (int32_2 < inputSize && !flag1)
              {
                if (index2 > 589824)
                  return;
                uint original = 0;
                uint vlBytes1 = MidiParse.GetVLBytes(inputMID, ref int32_2, ref original, ref altPattern, ref altOffset, ref altLength, true);
                trackEventArray[index2].deltaTime += vlBytes1;
                num7 += vlBytes1;
                trackEventArray[index2].absoluteTime = num7;
                byte num10 = MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, true);
                bool flag2 = num10 < (byte) 128;
                if (num10 == byte.MaxValue || flag2 && num9 == (int) byte.MaxValue)
                {
                  switch (!flag2 ? MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, true) : num10)
                  {
                    case 45:
                      int num11 = (int) MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, true);
                      int num12 = (int) MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, true);
                      int num13 = (int) MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, false);
                      int num14 = (int) MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, false);
                      int num15 = (int) MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, false);
                      int num16 = (int) MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, false);
                      break;
                    case 46:
                      int num17 = (int) MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, true);
                      if (MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, true) == byte.MaxValue)
                        break;
                      break;
                    case 47:
                      trackEventArray[index2].type = byte.MaxValue;
                      trackEventArray[index2].contentSize = 2;
                      trackEventArray[index2].contents = new byte[trackEventArray[index2].contentSize];
                      trackEventArray[index2].contents[0] = (byte) 47;
                      trackEventArray[index2].contents[1] = (byte) 0;
                      ++index2;
                      flag1 = true;
                      break;
                    case 81:
                      int num18 = ((int) MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, true) << 8 | (int) MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, true)) << 8 | (int) MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, true);
                      trackEventArray[index2].type = byte.MaxValue;
                      trackEventArray[index2].contentSize = 5;
                      trackEventArray[index2].contents = new byte[trackEventArray[index2].contentSize];
                      trackEventArray[index2].contents[0] = (byte) 81;
                      trackEventArray[index2].contents[1] = (byte) 3;
                      trackEventArray[index2].contents[2] = Convert.ToByte(num18 >> 16 & (int) byte.MaxValue);
                      trackEventArray[index2].contents[3] = Convert.ToByte(num18 >> 8 & (int) byte.MaxValue);
                      trackEventArray[index2].contents[4] = Convert.ToByte(num18 & (int) byte.MaxValue);
                      ++index2;
                      double num19 = 60000000.0 / (double) num18;
                      break;
                  }
                  if (!flag2)
                    num9 = (int) num10;
                }
                else if (num10 >= (byte) 144 && num10 < (byte) 160 || flag2 && num9 >= 144 && num9 < 160)
                {
                  byte num20;
                  if (flag2)
                  {
                    trackEventArray[index2].type = Convert.ToByte(num9);
                    num20 = num10;
                    int num21 = (int) Convert.ToByte(num9);
                  }
                  else
                  {
                    trackEventArray[index2].type = num10;
                    num20 = MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, true);
                  }
                  byte num22 = MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, true);
                  uint vlBytes2 = MidiParse.GetVLBytes(inputMID, ref int32_2, ref original, ref altPattern, ref altOffset, ref altLength, true);
                  trackEventArray[index2].durationTime = vlBytes2;
                  trackEventArray[index2].contentSize = 2;
                  trackEventArray[index2].contents = new byte[trackEventArray[index2].contentSize];
                  trackEventArray[index2].contents[0] = num20;
                  trackEventArray[index2].contents[1] = num22;
                  ++index2;
                  if (!flag2)
                    num9 = (int) num10;
                }
                else if (num10 >= (byte) 176 && num10 < (byte) 192 || flag2 && num9 >= 176 && num9 < 192)
                {
                  byte num23;
                  if (flag2)
                  {
                    num23 = num10;
                    trackEventArray[index2].type = Convert.ToByte(num9);
                  }
                  else
                  {
                    num23 = MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, true);
                    trackEventArray[index2].type = num10;
                  }
                  byte num24 = MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, true);
                  trackEventArray[index2].contentSize = 2;
                  trackEventArray[index2].contents = new byte[trackEventArray[index2].contentSize];
                  trackEventArray[index2].contents[0] = num23;
                  trackEventArray[index2].contents[1] = num24;
                  ++index2;
                  if (!flag2)
                    num9 = (int) num10;
                }
                else if (num10 >= (byte) 192 && num10 < (byte) 208 || flag2 && num9 >= 192 && num9 < 208)
                {
                  byte num25;
                  if (flag2)
                  {
                    num25 = num10;
                    trackEventArray[index2].type = Convert.ToByte(num9);
                  }
                  else
                  {
                    num25 = MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, true);
                    trackEventArray[index2].type = num10;
                  }
                  trackEventArray[index2].contentSize = 1;
                  trackEventArray[index2].contents = new byte[trackEventArray[index2].contentSize];
                  trackEventArray[index2].contents[0] = num25;
                  if ((int) num25 >= numberInstruments)
                    numberInstruments = (int) num25 + 1;
                  ++index2;
                  if (!flag2)
                    num9 = (int) num10;
                }
                else if (num10 >= (byte) 208 && num10 < (byte) 224 || flag2 && num9 >= 208 && num9 < 224)
                {
                  byte num26;
                  if (flag2)
                  {
                    num26 = num10;
                    trackEventArray[index2].type = Convert.ToByte(num9);
                  }
                  else
                  {
                    num26 = MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, true);
                    trackEventArray[index2].type = num10;
                  }
                  trackEventArray[index2].contentSize = 1;
                  trackEventArray[index2].contents = new byte[trackEventArray[index2].contentSize];
                  trackEventArray[index2].contents[0] = num26;
                  ++index2;
                  if (!flag2)
                    num9 = (int) num10;
                }
                else if (num10 >= (byte) 224 && num10 < (byte) 240 || flag2 && num9 >= 224 && num9 < 240)
                {
                  byte num27;
                  if (flag2)
                  {
                    num27 = num10;
                    trackEventArray[index2].type = Convert.ToByte(num9);
                  }
                  else
                  {
                    num27 = MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, true);
                    trackEventArray[index2].type = num10;
                  }
                  byte num28 = MidiParse.ReadMidiByte(inputMID, ref int32_2, ref altPattern, ref altOffset, ref altLength, true);
                  trackEventArray[index2].contentSize = 2;
                  trackEventArray[index2].contents = new byte[trackEventArray[index2].contentSize];
                  trackEventArray[index2].contents[0] = num27;
                  trackEventArray[index2].contents[1] = num28;
                  ++index2;
                  if (!flag2)
                    num9 = (int) num10;
                }
              }
              for (int index4 = 0; index4 < index2; ++index4)
              {
                if (index2 > 589824)
                  return;
                TrackEvent trackEvent = trackEventArray[index4];
                if (trackEvent.type >= (byte) 144 && trackEvent.type < (byte) 160 && trackEvent.durationTime > 0U)
                {
                  uint num29 = trackEvent.absoluteTime + trackEvent.durationTime;
                  if (index4 != index2 - 1)
                  {
                    for (int index5 = index4 + 1; index5 < index2; ++index5)
                    {
                      if (trackEventArray[index5].absoluteTime > num29 && index5 != index2 - 1)
                      {
                        for (int index6 = index2 - 1; index6 >= index5; --index6)
                        {
                          trackEventArray[index6 + 1].absoluteTime = trackEventArray[index6].absoluteTime;
                          trackEventArray[index6 + 1].contentSize = trackEventArray[index6].contentSize;
                          if (trackEventArray[index6 + 1].contents != null)
                            trackEventArray[index6 + 1].contents = (byte[]) null;
                          trackEventArray[index6 + 1].contents = new byte[trackEventArray[index6].contentSize];
                          for (int index7 = 0; index7 < trackEventArray[index6].contentSize; ++index7)
                            trackEventArray[index6 + 1].contents[index7] = trackEventArray[index6].contents[index7];
                          trackEventArray[index6 + 1].deltaTime = trackEventArray[index6].deltaTime;
                          trackEventArray[index6 + 1].durationTime = trackEventArray[index6].durationTime;
                          trackEventArray[index6 + 1].obsoleteEvent = trackEventArray[index6].obsoleteEvent;
                          trackEventArray[index6 + 1].type = trackEventArray[index6].type;
                        }
                        trackEventArray[index5].type = trackEventArray[index4].type;
                        trackEventArray[index5].absoluteTime = num29;
                        trackEventArray[index5].deltaTime = trackEventArray[index5].absoluteTime - trackEventArray[index5 - 1].absoluteTime;
                        trackEventArray[index5].contentSize = trackEventArray[index4].contentSize;
                        trackEventArray[index5].durationTime = 0U;
                        trackEventArray[index5].contents = new byte[trackEventArray[index5].contentSize];
                        trackEventArray[index5].contents[0] = trackEventArray[index4].contents[0];
                        trackEventArray[index5].contents[1] = (byte) 0;
                        trackEventArray[index5 + 1].deltaTime = trackEventArray[index5 + 1].absoluteTime - trackEventArray[index5].absoluteTime;
                        int deltaTime = (int) trackEventArray[index5].deltaTime;
                        ++index2;
                        break;
                      }
                      if (index5 == index2 - 1)
                      {
                        trackEventArray[index5 + 1].absoluteTime = num29;
                        trackEventArray[index5 + 1].contentSize = trackEventArray[index5].contentSize;
                        if (trackEventArray[index5 + 1].contents != null)
                          trackEventArray[index5 + 1].contents = (byte[]) null;
                        trackEventArray[index5 + 1].contents = new byte[trackEventArray[index5].contentSize];
                        for (int index8 = 0; index8 < trackEventArray[index5].contentSize; ++index8)
                          trackEventArray[index5 + 1].contents[index8] = trackEventArray[index5].contents[index8];
                        trackEventArray[index5 + 1].deltaTime = trackEventArray[index5].deltaTime;
                        trackEventArray[index5 + 1].durationTime = trackEventArray[index5].durationTime;
                        trackEventArray[index5 + 1].obsoleteEvent = trackEventArray[index5].obsoleteEvent;
                        trackEventArray[index5 + 1].type = trackEventArray[index5].type;
                        trackEventArray[index5].type = trackEventArray[index4].type;
                        trackEventArray[index5].absoluteTime = num29;
                        trackEventArray[index5].deltaTime = trackEventArray[index5].absoluteTime - trackEventArray[index5 - 1].absoluteTime;
                        trackEventArray[index5].contentSize = trackEventArray[index4].contentSize;
                        trackEventArray[index5].durationTime = 0U;
                        trackEventArray[index5].contents = new byte[trackEventArray[index5].contentSize];
                        trackEventArray[index5].contents[0] = trackEventArray[index4].contents[0];
                        trackEventArray[index5].contents[1] = (byte) 0;
                        trackEventArray[index5 + 1].deltaTime = trackEventArray[index5 + 1].absoluteTime - trackEventArray[index5].absoluteTime;
                        ++index2;
                        break;
                      }
                    }
                  }
                  else
                  {
                    trackEventArray[index4 + 1].absoluteTime = num29;
                    trackEventArray[index4 + 1].contentSize = trackEventArray[index4].contentSize;
                    if (trackEventArray[index4 + 1].contents != null)
                      trackEventArray[index4 + 1].contents = (byte[]) null;
                    trackEventArray[index4 + 1].contents = new byte[trackEventArray[index4].contentSize];
                    for (int index9 = 0; index9 < trackEventArray[index4].contentSize; ++index9)
                      trackEventArray[index4 + 1].contents[index9] = trackEventArray[index4].contents[index9];
                    trackEventArray[index4 + 1].deltaTime = trackEventArray[index4].deltaTime;
                    trackEventArray[index4 + 1].durationTime = trackEventArray[index4].durationTime;
                    trackEventArray[index4 + 1].obsoleteEvent = trackEventArray[index4].obsoleteEvent;
                    trackEventArray[index4 + 1].type = trackEventArray[index4].type;
                    trackEventArray[index4].type = trackEventArray[index4].type;
                    trackEventArray[index4].absoluteTime = num29;
                    int num30 = (int) trackEventArray[index4].absoluteTime - (int) trackEventArray[index4 - 1].absoluteTime;
                    trackEventArray[index4].deltaTime = trackEventArray[index4].absoluteTime - trackEventArray[index4 - 1].absoluteTime;
                    trackEventArray[index4].contentSize = trackEventArray[index4].contentSize;
                    trackEventArray[index4].durationTime = 0U;
                    trackEventArray[index4].contents = new byte[trackEventArray[index4].contentSize];
                    trackEventArray[index4].contents[0] = trackEventArray[index4].contents[0];
                    trackEventArray[index4].contents[1] = (byte) 0;
                    trackEventArray[index4 + 1].deltaTime = trackEventArray[index4 + 1].absoluteTime - trackEventArray[index4].absoluteTime;
                    int deltaTime = (int) trackEventArray[index4].deltaTime;
                    ++index2;
                  }
                }
              }
              uint num31 = 0;
              uint inLong = 0;
              byte num32 = 0;
              for (int index10 = 0; index10 < index2; ++index10)
              {
                TrackEvent trackEvent = trackEventArray[index10];
                if (trackEvent.obsoleteEvent)
                {
                  num31 += trackEvent.deltaTime;
                }
                else
                {
                  uint length = 0;
                  int num33 = (int) MidiParse.ReturnVLBytes(trackEvent.deltaTime + num31, ref length);
                  num31 = 0U;
                  uint num34 = inLong + length;
                  if ((int) trackEvent.type != (int) num32 || trackEvent.type == byte.MaxValue)
                    ++num34;
                  inLong = num34 + Convert.ToUInt32(trackEvent.contentSize);
                  num32 = trackEvent.type;
                }
              }
              uint num35 = MidiParse.Flip32Bit(inLong);
              outFile.Write(BitConverter.GetBytes(num35), 0, 4);
              uint num36 = 0;
              byte num37 = 0;
              for (int index11 = 0; index11 < index2; ++index11)
              {
                TrackEvent trackEvent = trackEventArray[index11];
                if (trackEvent.obsoleteEvent)
                {
                  num36 += trackEvent.deltaTime;
                }
                else
                {
                  uint length = 0;
                  uint num38 = MidiParse.ReturnVLBytes(trackEvent.deltaTime + num36, ref length);
                  num36 = 0U;
                  MidiParse.WriteVLBytes(outFile, num38, length, true);
                  if ((int) trackEvent.type != (int) num37 || trackEvent.type == byte.MaxValue)
                    outFile.WriteByte(trackEvent.type);
                  outFile.Write(trackEvent.contents, 0, trackEvent.contentSize);
                  num37 = trackEvent.type;
                }
              }
              for (int index12 = 0; index12 < index2; ++index12)
              {
                if (trackEventArray[index12].contents != null)
                  trackEventArray[index12].contents = (byte[]) null;
              }
            }
            ++num6;
          }
          outFile.Close();
          outFile.Dispose();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error exporting " + ex.ToString(), "Error");
      }
    }

    public static bool MidiToGEFormat(
      string input,
      string output,
      bool loop,
      uint loopPoint,
      bool useRepeaters)
    {
      try
      {
        TrackEvent[,] trackEventArray = new TrackEvent[32, 65536];
        for (int index1 = 0; index1 < 32; ++index1)
        {
          for (int index2 = 0; index2 < 65536; ++index2)
            trackEventArray[index1, index2] = new TrackEvent();
        }
        int[] numArray1 = new int[32];
        for (int index = 0; index < 32; ++index)
          numArray1[index] = 0;
        string str = input;
        int int32_1 = Convert.ToInt32(new FileInfo(str).Length);
        byte[] numArray2 = File.ReadAllBytes(str);
        if (MidiParse.CharArrayToLong(numArray2, 0) != 1297377380U)
        {
          int num = (int) MessageBox.Show("Invalid midi hdr", "Error");
          return false;
        }
        int num1 = (int) MidiParse.CharArrayToLong(numArray2, 4);
        ushort num2 = MidiParse.CharArrayToShort(numArray2, 8);
        ushort num3 = MidiParse.CharArrayToShort(numArray2, 10);
        ushort num4 = MidiParse.CharArrayToShort(numArray2, 12);
        if (num3 > (ushort) 16)
        {
          int num5 = (int) MessageBox.Show("Too many tracks, truncated to 16", "Warning");
          num3 = (ushort) 16;
        }
        FileStream outFile = new FileStream(output, FileMode.Create, FileAccess.Write);
        if (outFile == null)
        {
          int num6 = (int) MessageBox.Show("Error outputting file", "Error");
          return false;
        }
        int num7 = (int) num3;
        if (num2 != (ushort) 0 && num2 != (ushort) 1)
        {
          int num8 = (int) MessageBox.Show("Invalid midi type", "Error");
          return false;
        }
        int offset = 14;
        byte[] altPattern = (byte[]) null;
        byte altOffset = 0;
        byte altLength = 0;
        bool flag1 = false;
        uint num9 = 0;
        uint[] numArray3 = new uint[16];
        for (int index = 0; index < 16; ++index)
          numArray3[index] = 0U;
        for (int index3 = 0; index3 < num7; ++index3)
        {
          uint num10 = 0;
          if (((((int) numArray2[offset] << 8 | (int) numArray2[offset + 1]) << 8 | (int) numArray2[offset + 2]) << 8 | (int) numArray2[offset + 3]) != 1297379947)
          {
            int num11 = (int) MessageBox.Show("Invalid track midi hdr", "Error");
            return false;
          }
          int num12 = (int) numArray2[offset + 4];
          int num13 = (int) numArray2[offset + 5];
          int num14 = (int) numArray2[offset + 6];
          int num15 = (int) numArray2[offset + 7];
          offset += 8;
          byte num16 = byte.MaxValue;
          bool flag2 = false;
          while (!flag2 && offset < int32_1)
          {
            uint original = 0;
            uint vlBytes = MidiParse.GetVLBytes(numArray2, ref offset, ref original, ref altPattern, ref altOffset, ref altLength, false);
            num10 += vlBytes;
            byte num17 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
            bool flag3 = num17 <= (byte) 127;
            if (num17 == byte.MaxValue)
            {
              byte num18 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              switch (num18)
              {
                case 47:
                  num10 -= vlBytes;
                  flag2 = true;
                  int num19 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  break;
                case 81:
                  int num20 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  int num21 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  int num22 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  int num23 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  break;
                default:
                  if (num18 < (byte) 127 && num18 != (byte) 81 && num18 != (byte) 47)
                  {
                    uint num24 = (uint) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                    for (int index4 = 0; (long) index4 < (long) num24; ++index4)
                    {
                      int num25 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                    }
                    break;
                  }
                  if (num18 == (byte) 127)
                  {
                    int int32_2 = Convert.ToInt32(MidiParse.GetVLBytes(numArray2, ref offset, ref original, ref altPattern, ref altOffset, ref altLength, false));
                    for (int index5 = 0; index5 < int32_2; ++index5)
                    {
                      int num26 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                    }
                    break;
                  }
                  break;
              }
              num16 = num17;
            }
            else if (num17 >= (byte) 128 && num17 < (byte) 144 || flag3 && num16 >= (byte) 128 && num16 < (byte) 144)
            {
              if (!flag3)
              {
                int num27 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              }
              int num28 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              if (!flag3)
                num16 = num17;
            }
            else if (num17 >= (byte) 144 && num17 < (byte) 160 || flag3 && num16 >= (byte) 144 && num16 < (byte) 160)
            {
              if (!flag3)
              {
                int num29 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              }
              int num30 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              if (!flag3)
                num16 = num17;
            }
            else if (num17 >= (byte) 176 && num17 < (byte) 192 || flag3 && num16 >= (byte) 176 && num16 < (byte) 192)
            {
              if (!flag3)
              {
                int num31 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              }
              int num32 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              if (!flag3)
                num16 = num17;
            }
            else if (num17 >= (byte) 192 && num17 < (byte) 208 || flag3 && num16 >= (byte) 192 && num16 < (byte) 208)
            {
              if (!flag3)
              {
                int num33 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              }
              if (!flag3)
                num16 = num17;
            }
            else if (num17 >= (byte) 208 && num17 < (byte) 224 || flag3 && num16 >= (byte) 208 && num16 < (byte) 224)
            {
              if (!flag3)
              {
                int num34 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              }
              if (!flag3)
                num16 = num17;
            }
            else if (num17 >= (byte) 224 && num17 < (byte) 240 || flag3 && num16 >= (byte) 224 && num16 < (byte) 240)
            {
              if (!flag3)
              {
                int num35 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              }
              int num36 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              if (!flag3)
                num16 = num17;
            }
            else if (num17 == (byte) 240 || num17 == (byte) 247)
            {
              int int32_3 = Convert.ToInt32(MidiParse.GetVLBytes(numArray2, ref offset, ref original, ref altPattern, ref altOffset, ref altLength, false));
              for (int index6 = 0; index6 < int32_3; ++index6)
              {
                int num37 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              }
            }
            else if (!flag1)
            {
              int num38 = (int) MessageBox.Show("Invalid midi character found", "Error");
              flag1 = true;
            }
          }
          if (num10 > num9)
            num9 = num10;
          if (num10 > numArray3[index3])
            numArray3[index3] = num10;
        }
        offset = 14;
        altPattern = (byte[]) null;
        altOffset = (byte) 0;
        altLength = (byte) 0;
        for (int index7 = 0; index7 < num7; ++index7)
        {
          uint num39 = 0;
          if (((((int) numArray2[offset] << 8 | (int) numArray2[offset + 1]) << 8 | (int) numArray2[offset + 2]) << 8 | (int) numArray2[offset + 3]) != 1297379947)
          {
            int num40 = (int) MessageBox.Show("Invalid track midi hdr", "Error");
            return false;
          }
          int num41 = (int) numArray2[offset + 4];
          int num42 = (int) numArray2[offset + 5];
          int num43 = (int) numArray2[offset + 6];
          int num44 = (int) numArray2[offset + 7];
          offset += 8;
          byte num45 = byte.MaxValue;
          bool flag4 = false;
          bool flag5 = false;
          if (loop && loopPoint == 0U && numArray3[index7] > 0U)
          {
            TrackEvent trackEvent = trackEventArray[index7, numArray1[index7]];
            trackEvent.type = byte.MaxValue;
            trackEvent.absoluteTime = 0U;
            trackEvent.contentSize = 3;
            trackEvent.contents = new byte[trackEvent.contentSize];
            trackEvent.contents[0] = (byte) 46;
            trackEvent.contents[1] = (byte) 0;
            trackEvent.contents[2] = byte.MaxValue;
            trackEvent.deltaTime = 0U;
            trackEvent.obsoleteEvent = false;
            ++numArray1[index7];
            flag5 = true;
          }
          while (!flag4 && offset < int32_1)
          {
            uint original = 0;
            uint vlBytes = MidiParse.GetVLBytes(numArray2, ref offset, ref original, ref altPattern, ref altOffset, ref altLength, false);
            num39 += vlBytes;
            TrackEvent trackEvent1 = trackEventArray[index7, numArray1[index7]];
            trackEvent1.deltaTime = vlBytes;
            trackEvent1.obsoleteEvent = false;
            trackEvent1.contents = (byte[]) null;
            trackEvent1.absoluteTime = num39;
            if (loop && !flag5 && numArray3[index7] > loopPoint)
            {
              if ((int) num39 == (int) loopPoint)
              {
                TrackEvent trackEvent2 = trackEventArray[index7, numArray1[index7]];
                trackEvent2.type = byte.MaxValue;
                trackEvent2.absoluteTime = num39;
                trackEvent2.contentSize = 3;
                trackEvent2.contents = new byte[trackEvent2.contentSize];
                trackEvent2.contents[0] = (byte) 46;
                trackEvent2.contents[1] = (byte) 0;
                trackEvent2.contents[2] = byte.MaxValue;
                trackEvent2.deltaTime = vlBytes;
                trackEvent2.obsoleteEvent = false;
                ++numArray1[index7];
                trackEvent1 = trackEventArray[index7, numArray1[index7]];
                trackEvent1.deltaTime = 0U;
                trackEvent1.obsoleteEvent = false;
                trackEvent1.contents = (byte[]) null;
                trackEvent1.absoluteTime = num39;
                flag5 = true;
              }
              else if (num39 > loopPoint)
              {
                TrackEvent trackEvent3 = trackEventArray[index7, numArray1[index7]];
                trackEvent3.type = byte.MaxValue;
                trackEvent3.absoluteTime = loopPoint;
                trackEvent3.contentSize = 3;
                trackEvent3.contents = new byte[trackEvent3.contentSize];
                trackEvent3.contents[0] = (byte) 46;
                trackEvent3.contents[1] = (byte) 0;
                trackEvent3.contents[2] = byte.MaxValue;
                trackEvent3.deltaTime = numArray1[index7] <= 0 ? loopPoint : loopPoint - trackEventArray[index7, numArray1[index7] - 1].absoluteTime;
                trackEvent3.obsoleteEvent = false;
                ++numArray1[index7];
                trackEvent1 = trackEventArray[index7, numArray1[index7]];
                trackEvent1.deltaTime = num39 - loopPoint;
                trackEvent1.obsoleteEvent = false;
                trackEvent1.contents = (byte[]) null;
                trackEvent1.absoluteTime = num39;
                flag5 = true;
              }
            }
            byte num46 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
            bool flag6 = num46 <= (byte) 127;
            if (num46 == byte.MaxValue)
            {
              byte num47 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              switch (num47)
              {
                case 47:
                  flag4 = true;
                  if (loop && numArray3[index7] > loopPoint)
                  {
                    TrackEvent trackEvent4 = trackEventArray[index7, numArray1[index7] - 1];
                    if (trackEvent4.type == byte.MaxValue && trackEvent4.contentSize > 0 && trackEvent4.contents[0] == (byte) 46)
                    {
                      TrackEvent trackEvent5 = trackEvent4;
                      trackEvent5.type = byte.MaxValue;
                      trackEvent5.contentSize = 1;
                      trackEvent5.contents = new byte[trackEvent5.contentSize];
                      trackEvent5.contents[0] = (byte) 47;
                    }
                    else
                    {
                      TrackEvent trackEvent6 = trackEventArray[index7, numArray1[index7] + 1];
                      trackEvent6.absoluteTime = num9;
                      trackEvent6.deltaTime = 0U;
                      trackEvent6.durationTime = trackEvent1.durationTime;
                      trackEvent6.obsoleteEvent = trackEvent1.obsoleteEvent;
                      trackEvent6.type = byte.MaxValue;
                      trackEvent6.contentSize = 1;
                      trackEvent6.contents = new byte[trackEvent6.contentSize];
                      trackEvent6.contents[0] = (byte) 47;
                      trackEvent1.type = byte.MaxValue;
                      if (num9 > trackEvent4.absoluteTime + trackEvent4.durationTime)
                      {
                        trackEvent1.deltaTime = num9 - (trackEvent4.absoluteTime + trackEvent4.durationTime);
                        trackEvent1.absoluteTime = num9;
                      }
                      else
                      {
                        trackEvent1.deltaTime = 0U;
                        trackEvent1.absoluteTime = trackEvent4.absoluteTime;
                      }
                      trackEvent1.contentSize = 7;
                      trackEvent1.contents = new byte[trackEvent1.contentSize];
                      trackEvent1.contents[0] = (byte) 45;
                      trackEvent1.contents[1] = byte.MaxValue;
                      trackEvent1.contents[2] = byte.MaxValue;
                      trackEvent1.contents[3] = (byte) 0;
                      trackEvent1.contents[4] = (byte) 0;
                      trackEvent1.contents[5] = (byte) 0;
                      trackEvent1.contents[6] = (byte) 0;
                      trackEvent1.obsoleteEvent = false;
                      ++numArray1[index7];
                    }
                  }
                  else
                  {
                    trackEvent1.type = byte.MaxValue;
                    trackEvent1.contentSize = 1;
                    trackEvent1.contents = new byte[trackEvent1.contentSize];
                    trackEvent1.contents[0] = (byte) 47;
                  }
                  int num48 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  break;
                case 81:
                  trackEvent1.type = byte.MaxValue;
                  trackEvent1.contentSize = 4;
                  trackEvent1.contents = new byte[trackEvent1.contentSize];
                  trackEvent1.contents[0] = (byte) 81;
                  int num49 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  trackEvent1.contents[1] = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  trackEvent1.contents[2] = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  trackEvent1.contents[3] = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  break;
                default:
                  if (num47 < (byte) 127 && num47 != (byte) 81 && num47 != (byte) 47)
                  {
                    trackEvent1.type = byte.MaxValue;
                    uint num50 = (uint) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                    for (int index8 = 0; (long) index8 < (long) num50; ++index8)
                    {
                      int num51 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                    }
                    trackEvent1.obsoleteEvent = true;
                    break;
                  }
                  if (num47 == (byte) 127)
                  {
                    trackEvent1.type = byte.MaxValue;
                    int int32_4 = Convert.ToInt32(MidiParse.GetVLBytes(numArray2, ref offset, ref original, ref altPattern, ref altOffset, ref altLength, false));
                    for (int index9 = 0; index9 < int32_4; ++index9)
                    {
                      int num52 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                    }
                    trackEvent1.obsoleteEvent = true;
                    break;
                  }
                  break;
              }
              num45 = num46;
            }
            else if (num46 >= (byte) 128 && num46 < (byte) 144 || flag6 && num45 >= (byte) 128 && num45 < (byte) 144)
            {
              byte num53;
              byte num54;
              if (flag6)
              {
                trackEvent1.type = num45;
                num53 = num46;
                num54 = num45;
              }
              else
              {
                trackEvent1.type = num46;
                num53 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                num54 = num46;
              }
              byte num55 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              for (int index10 = numArray1[index7] - 1; index10 >= 0; --index10)
              {
                if ((int) trackEventArray[index7, index10].type == (144 | (int) num54 & 15) && !trackEventArray[index7, index10].obsoleteEvent && (int) trackEventArray[index7, index10].contents[0] == (int) num53)
                {
                  trackEventArray[index7, index10].durationTime = num39 - trackEventArray[index7, index10].absoluteTime;
                  break;
                }
              }
              trackEvent1.durationTime = 0U;
              trackEvent1.contentSize = 2;
              trackEvent1.contents = new byte[trackEvent1.contentSize];
              trackEvent1.contents[0] = num53;
              trackEvent1.contents[1] = num55;
              trackEvent1.obsoleteEvent = true;
              if (!flag6)
                num45 = num46;
            }
            else if (num46 >= (byte) 144 && num46 < (byte) 160 || flag6 && num45 >= (byte) 144 && num45 < (byte) 160)
            {
              byte num56;
              byte num57;
              if (flag6)
              {
                trackEvent1.type = num45;
                num56 = num46;
                num57 = num45;
              }
              else
              {
                trackEvent1.type = num46;
                num56 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                num57 = num46;
              }
              byte num58 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              if (num58 == (byte) 0)
              {
                for (int index11 = numArray1[index7] - 1; index11 >= 0; --index11)
                {
                  if ((int) trackEventArray[index7, index11].type == (int) num57 && !trackEventArray[index7, index11].obsoleteEvent && (int) trackEventArray[index7, index11].contents[0] == (int) num56)
                  {
                    trackEventArray[index7, index11].durationTime = num39 - trackEventArray[index7, index11].absoluteTime;
                    break;
                  }
                }
                trackEvent1.durationTime = 0U;
                trackEvent1.contentSize = 2;
                trackEvent1.contents = new byte[trackEvent1.contentSize];
                trackEvent1.contents[0] = num56;
                trackEvent1.contents[1] = num58;
                trackEvent1.obsoleteEvent = true;
              }
              else
              {
                for (int index12 = numArray1[index7] - 1; index12 >= 0; --index12)
                {
                  if ((int) trackEventArray[index7, index12].type == (int) num57 && !trackEventArray[index7, index12].obsoleteEvent && (int) trackEventArray[index7, index12].contents[0] == (int) num56)
                  {
                    if (trackEventArray[index7, index12].durationTime == 0U)
                    {
                      trackEventArray[index7, index12].durationTime = num39 - trackEventArray[index7, index12].absoluteTime;
                      break;
                    }
                    break;
                  }
                }
                trackEvent1.durationTime = 0U;
                trackEvent1.contentSize = 2;
                trackEvent1.contents = new byte[trackEvent1.contentSize];
                trackEvent1.contents[0] = num56;
                trackEvent1.contents[1] = num58;
              }
              if (!flag6)
                num45 = num46;
            }
            else if (num46 >= (byte) 176 && num46 < (byte) 192 || flag6 && num45 >= (byte) 176 && num45 < (byte) 192)
            {
              byte num59;
              if (flag6)
              {
                num59 = num46;
                trackEvent1.type = num45;
              }
              else
              {
                num59 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                trackEvent1.type = num46;
              }
              byte num60 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              trackEvent1.contentSize = 2;
              trackEvent1.contents = new byte[trackEvent1.contentSize];
              trackEvent1.contents[0] = num59;
              trackEvent1.contents[1] = num60;
              if (!flag6)
                num45 = num46;
            }
            else if (num46 >= (byte) 192 && num46 < (byte) 208 || flag6 && num45 >= (byte) 192 && num45 < (byte) 208)
            {
              byte num61;
              if (flag6)
              {
                num61 = num46;
                trackEvent1.type = num45;
              }
              else
              {
                num61 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                trackEvent1.type = num46;
              }
              trackEvent1.contentSize = 1;
              trackEvent1.contents = new byte[trackEvent1.contentSize];
              trackEvent1.contents[0] = num61;
              if (!flag6)
                num45 = num46;
            }
            else if (num46 >= (byte) 208 && num46 < (byte) 224 || flag6 && num45 >= (byte) 208 && num45 < (byte) 224)
            {
              trackEvent1.type = num46;
              byte num62;
              if (flag6)
              {
                num62 = num46;
                trackEvent1.type = num45;
              }
              else
              {
                num62 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                trackEvent1.type = num46;
              }
              trackEvent1.contentSize = 1;
              trackEvent1.contents = new byte[trackEvent1.contentSize];
              trackEvent1.contents[0] = num62;
              if (!flag6)
                num45 = num46;
            }
            else if (num46 >= (byte) 224 && num46 < (byte) 240 || flag6 && num45 >= (byte) 224 && num45 < (byte) 240)
            {
              trackEvent1.type = num46;
              byte num63;
              if (flag6)
              {
                num63 = num46;
                trackEvent1.type = num45;
              }
              else
              {
                num63 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                trackEvent1.type = num46;
              }
              byte num64 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              trackEvent1.contentSize = 2;
              trackEvent1.contents = new byte[trackEvent1.contentSize];
              trackEvent1.contents[0] = num63;
              trackEvent1.contents[1] = num64;
              if (!flag6)
                num45 = num46;
            }
            else if (num46 == (byte) 240 || num46 == (byte) 247)
            {
              trackEvent1.type = num46;
              int int32_5 = Convert.ToInt32(MidiParse.GetVLBytes(numArray2, ref offset, ref original, ref altPattern, ref altOffset, ref altLength, false));
              for (int index13 = 0; index13 < int32_5; ++index13)
              {
                int num65 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              }
              trackEvent1.obsoleteEvent = true;
            }
            else if (!flag1)
            {
              int num66 = (int) MessageBox.Show("Invalid midi character found", "Error");
              flag1 = true;
            }
            ++numArray1[index7];
          }
        }
        uint num67 = 0;
        uint inLong = 68;
        for (int index14 = 0; index14 < num7; ++index14)
        {
          uint num68 = 0;
          int num69 = 0;
          byte num70 = 0;
          if (numArray1[index14] > 0)
          {
            uint num71 = MidiParse.Flip32Bit(inLong);
            outFile.Write(BitConverter.GetBytes(num71), 0, 4);
            for (int index15 = 0; index15 < numArray1[index14]; ++index15)
            {
              TrackEvent trackEvent = trackEventArray[index14, index15];
              uint length1 = 0;
              int num72 = (int) MidiParse.ReturnVLBytes(trackEvent.deltaTime + num67, ref length1);
              if (trackEvent.obsoleteEvent)
              {
                num67 += trackEvent.deltaTime;
              }
              else
              {
                if (trackEvent.type == byte.MaxValue && trackEvent.contents[0] == (byte) 46)
                  num69 = Convert.ToInt32((long) (uint) ((int) inLong + (int) num68 + 1) + (long) trackEvent.contentSize + (long) length1);
                num67 = 0U;
                uint num73 = num68 + length1;
                if (trackEvent.type == byte.MaxValue && trackEvent.contents[0] == (byte) 45)
                {
                  uint uint32 = Convert.ToUInt32((long) (inLong + num73) - (long) num69 + 8L);
                  trackEvent.contents[3] = Convert.ToByte(uint32 >> 24 & (uint) byte.MaxValue);
                  trackEvent.contents[4] = Convert.ToByte(uint32 >> 16 & (uint) byte.MaxValue);
                  trackEvent.contents[5] = Convert.ToByte(uint32 >> 8 & (uint) byte.MaxValue);
                  trackEvent.contents[6] = Convert.ToByte(uint32 & (uint) byte.MaxValue);
                }
                if ((int) trackEvent.type != (int) num70 || trackEvent.type == byte.MaxValue)
                  ++num73;
                num68 = num73 + Convert.ToUInt32(trackEvent.contentSize);
                if (trackEvent.type >= (byte) 144 && trackEvent.type < (byte) 160)
                {
                  uint length2 = 0;
                  int num74 = (int) MidiParse.ReturnVLBytes(trackEvent.durationTime, ref length2);
                  num68 += length2;
                }
                num70 = trackEvent.type;
              }
            }
            inLong += num68;
          }
          else
          {
            uint num75 = 0;
            outFile.Write(BitConverter.GetBytes(num75), 0, 4);
          }
        }
        for (int index = num7; index < 16; ++index)
        {
          uint num76 = 0;
          outFile.Write(BitConverter.GetBytes(num76), 0, 4);
        }
        uint num77 = MidiParse.Flip32Bit((uint) num4);
        outFile.Write(BitConverter.GetBytes(num77), 0, 4);
        for (int index16 = 0; index16 < num7; ++index16)
        {
          if (numArray1[index16] > 0)
          {
            byte num78 = 0;
            for (int index17 = 0; index17 < numArray1[index16]; ++index17)
            {
              TrackEvent trackEvent = trackEventArray[index16, index17];
              if (trackEvent.obsoleteEvent)
              {
                num67 += trackEvent.deltaTime;
              }
              else
              {
                uint length3 = 0;
                uint num79 = MidiParse.ReturnVLBytes(trackEvent.deltaTime + num67, ref length3);
                num67 = 0U;
                MidiParse.WriteVLBytes(outFile, num79, length3, false);
                if ((int) trackEvent.type != (int) num78 || trackEvent.type == byte.MaxValue)
                  outFile.WriteByte(trackEvent.type);
                outFile.Write(trackEvent.contents, 0, trackEvent.contentSize);
                if (trackEvent.type >= (byte) 144 && trackEvent.type < (byte) 160)
                {
                  uint length4 = 0;
                  uint num80 = MidiParse.ReturnVLBytes(trackEvent.durationTime, ref length4);
                  MidiParse.WriteVLBytes(outFile, num80, length4, false);
                }
                num78 = trackEvent.type;
              }
            }
          }
          for (int index18 = 0; index18 < numArray1[index16]; ++index18)
          {
            if (trackEventArray[index16, index18].contents != null)
              trackEventArray[index16, index18].contents = (byte[]) null;
          }
        }
        outFile.Close();
        outFile.Dispose();
        int int32_6 = Convert.ToInt32(new FileInfo(output).Length);
        byte[] Buffer1 = File.ReadAllBytes(output);
        uint[] numArray4 = new uint[16];
        int[] numArray5 = new int[16];
        for (int index = 0; index < 64; index += 4)
        {
          numArray4[index / 4] = (uint) ((((int) Buffer1[index] << 8 | (int) Buffer1[index + 1]) << 8 | (int) Buffer1[index + 2]) << 8) | (uint) Buffer1[index + 3];
          numArray5[index / 4] = 0;
        }
        for (int index19 = 0; index19 < int32_6; ++index19)
        {
          if (index19 > 68 && Buffer1[index19] == (byte) 254)
          {
            for (int index20 = 0; index20 < num7; ++index20)
            {
              if ((long) numArray4[index20] > (long) index19)
                ++numArray5[index20];
            }
          }
        }
        FileStream fileStream1 = new FileStream(output, FileMode.Create, FileAccess.Write);
        for (int index = 0; index < 16; ++index)
          MidiParse.WriteLongToBuffer(Buffer1, Convert.ToUInt32(index * 4), numArray4[index] + Convert.ToUInt32(numArray5[index]));
        for (int index = 0; index < int32_6; ++index)
        {
          fileStream1.WriteByte(Buffer1[index]);
          if (index > 68 && Buffer1[index] == (byte) 254)
            fileStream1.WriteByte(Buffer1[index]);
        }
        fileStream1.Close();
        fileStream1.Dispose();
        byte[] numArray6 = (byte[]) null;
        if (useRepeaters)
        {
          int int32_7 = Convert.ToInt32(new FileInfo(output).Length);
          byte[] numArray7 = File.ReadAllBytes(output);
          uint[] numArray8 = new uint[16];
          for (int index = 0; index < 64; index += 4)
            numArray8[index / 4] = (uint) ((((int) numArray7[index] << 8 | (int) numArray7[index + 1]) << 8 | (int) numArray7[index + 2]) << 8) | (uint) numArray7[index + 3];
          uint data = (uint) ((((int) numArray7[64] << 8 | (int) numArray7[65]) << 8 | (int) numArray7[66]) << 8) | (uint) numArray7[67];
          byte[] Buffer2 = new byte[int32_7 * 4];
          for (int index = 0; index < int32_7 * 4; ++index)
            Buffer2[index] = (byte) 0;
          uint[] numArray9 = new uint[16];
          for (int index = 0; index < 16; ++index)
            numArray9[index] = 0U;
          int num81 = 68;
          for (int index21 = 0; index21 < 16 && numArray8[index21] != 0U; ++index21)
          {
            numArray9[index21] = Convert.ToUInt32(num81);
            int num82 = num81;
            int num83 = int32_7;
            if (index21 < 15 && numArray8[index21 + 1] != 0U)
              num83 = Convert.ToInt32(numArray8[index21 + 1]);
            int int32_8 = Convert.ToInt32(numArray8[index21]);
            while (int32_8 < num83)
            {
              int num84 = -1;
              int num85 = -1;
              for (int index22 = num82; index22 < num81; ++index22)
              {
                int num86;
                for (num86 = 0; (int) Buffer2[index22 + num86] == (int) numArray7[int32_8 + num86] && int32_8 + num86 < num83 && Buffer2[index22 + num86] != (byte) 254 && Buffer2[index22 + num86] != byte.MaxValue && index22 + num86 < num81; ++num86)
                {
                  bool flag7 = false;
                  for (int index23 = int32_8 + num86; index23 < num83 && index23 < int32_8 + num86 + 5; ++index23)
                  {
                    if (numArray7[index23] == byte.MaxValue)
                      flag7 = true;
                  }
                  if (flag7)
                    break;
                }
                if (num86 > num85 && num86 > 6)
                {
                  num85 = num86;
                  num84 = index22;
                }
              }
              Convert.ToInt32(((long) int32_8 - (long) numArray8[index21]) / 2L);
              if (num85 > 6)
              {
                if (num85 > 253)
                  num85 = 253;
                byte[] numArray10 = Buffer2;
                int index24 = num81;
                int num87 = index24 + 1;
                numArray10[index24] = (byte) 254;
                int num88 = num87 - num84 - 1;
                byte[] numArray11 = Buffer2;
                int index25 = num87;
                int num89 = index25 + 1;
                int num90 = (int) Convert.ToByte(num88 >> 8 & (int) byte.MaxValue);
                numArray11[index25] = (byte) num90;
                byte[] numArray12 = Buffer2;
                int index26 = num89;
                int num91 = index26 + 1;
                int num92 = (int) Convert.ToByte(num88 & (int) byte.MaxValue);
                numArray12[index26] = (byte) num92;
                byte[] numArray13 = Buffer2;
                int index27 = num91;
                num81 = index27 + 1;
                int num93 = (int) Convert.ToByte(num85);
                numArray13[index27] = (byte) num93;
                int32_8 += num85;
              }
              else
              {
                Buffer2[num81++] = numArray7[int32_8];
                ++int32_8;
              }
            }
            if (num81 % 4 != 0)
              num81 += 4 - num81 % 4;
          }
          for (int index = 0; index < 16; ++index)
          {
            if (numArray9[index] != 0U)
            {
              Convert.ToInt32(numArray9[index]);
              int num94 = num81;
              if (index < 15 && numArray9[index + 1] != 0U)
                num94 = Convert.ToInt32(numArray9[index + 1]);
              int int32_9 = Convert.ToInt32(numArray9[index]);
              bool flag8 = false;
              int num95 = 0;
              while (int32_9 < num94)
              {
                if (Buffer2[int32_9] == byte.MaxValue && Buffer2[int32_9 + 1] == (byte) 46 && Buffer2[int32_9 + 2] == (byte) 0 && Buffer2[int32_9 + 3] == byte.MaxValue)
                {
                  flag8 = true;
                  num95 = int32_9 + 4;
                  int32_9 += 4;
                }
                else if (Buffer2[int32_9] == byte.MaxValue && Buffer2[int32_9 + 1] == (byte) 45 && Buffer2[int32_9 + 2] == byte.MaxValue && Buffer2[int32_9 + 3] == byte.MaxValue)
                {
                  if (flag8)
                  {
                    int num96 = int32_9 + 8 - num95;
                    MidiParse.WriteLongToBuffer(Buffer2, Convert.ToUInt32(int32_9 + 4), Convert.ToUInt32(num96));
                    flag8 = false;
                  }
                  int32_9 += 8;
                }
                else
                  ++int32_9;
              }
            }
          }
          for (int index = 0; index < 16; ++index)
            MidiParse.WriteLongToBuffer(Buffer2, Convert.ToUInt32(index * 4), Convert.ToUInt32(numArray9[index]));
          MidiParse.WriteLongToBuffer(Buffer2, 64U, data);
          FileStream fileStream2 = new FileStream(output, FileMode.Create, FileAccess.Write);
          for (int index = 0; index < num81; ++index)
            fileStream2.WriteByte(Buffer2[index]);
          fileStream2.Close();
          fileStream2.Dispose();
          numArray6 = (byte[]) null;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error converting " + ex.ToString(), "Error");
        return false;
      }
      return true;
    }

    public static bool MidiToBTFormat(
      string input,
      string output,
      bool loop,
      uint loopPoint,
      bool useRepeaters)
    {
      string str = output + "temp.bin";
      ushort numTracks = 32;
      if (!MidiParse.MidiToBTFormatStageOne(input, str, loop, loopPoint, useRepeaters, ref numTracks))
        return false;
      int int32 = Convert.ToInt32(new FileInfo(str).Length);
      byte[] currentSpot = File.ReadAllBytes(str);
      File.Delete(str);
      FileStream fileStream = new FileStream(output, FileMode.Create, FileAccess.Write);
      uint inLong = MidiParse.CharArrayToLong(currentSpot, 128);
      uint[] numArray = new uint[32];
      for (int index = 0; index < 128; index += 4)
      {
        numArray[index / 4] = (uint) ((((int) currentSpot[index] << 8 | (int) currentSpot[index + 1]) << 8 | (int) currentSpot[index + 2]) << 8) | (uint) currentSpot[index + 3];
        if (numArray[index / 4] == 0U)
          break;
      }
      uint uint32 = Convert.ToUInt32(132 - ((int) numTracks * 4 + 8));
      uint num1 = MidiParse.Flip32Bit(inLong);
      fileStream.Write(BitConverter.GetBytes(num1), 0, 4);
      uint num2 = MidiParse.Flip32Bit((uint) numTracks);
      fileStream.Write(BitConverter.GetBytes(num2), 0, 4);
      for (int index = 0; index < (int) numTracks; ++index)
      {
        uint num3 = MidiParse.Flip32Bit(numArray[index] - uint32);
        fileStream.Write(BitConverter.GetBytes(num3), 0, 4);
      }
      for (int index = 132; index < int32; ++index)
        fileStream.WriteByte(currentSpot[index]);
      fileStream.Close();
      fileStream.Dispose();
      return true;
    }

    private static bool MidiToBTFormatStageOne(
      string input,
      string output,
      bool loop,
      uint loopPoint,
      bool useRepeaters,
      ref ushort numTracks)
    {
      try
      {
        TrackEvent[,] trackEventArray = new TrackEvent[32, 65536];
        for (int index1 = 0; index1 < 32; ++index1)
        {
          for (int index2 = 0; index2 < 65536; ++index2)
            trackEventArray[index1, index2] = new TrackEvent();
        }
        int[] numArray1 = new int[32];
        for (int index = 0; index < 32; ++index)
          numArray1[index] = 0;
        string str = input;
        int int32_1 = Convert.ToInt32(new FileInfo(str).Length);
        byte[] numArray2 = File.ReadAllBytes(str);
        if (MidiParse.CharArrayToLong(numArray2, 0) != 1297377380U)
        {
          int num = (int) MessageBox.Show("Invalid midi hdr", "Error");
          return false;
        }
        int num1 = (int) MidiParse.CharArrayToLong(numArray2, 4);
        ushort num2 = MidiParse.CharArrayToShort(numArray2, 8);
        numTracks = MidiParse.CharArrayToShort(numArray2, 10);
        ushort num3 = MidiParse.CharArrayToShort(numArray2, 12);
        if (numTracks > (ushort) 32)
        {
          int num4 = (int) MessageBox.Show("Too many tracks, truncated to 32", "Warning");
          numTracks = (ushort) 32;
        }
        int uint32_1 = (int) Convert.ToUInt32(132 - ((int) numTracks * 4 + 8));
        FileStream outFile = new FileStream(output, FileMode.Create, FileAccess.Write);
        if (outFile == null)
        {
          int num5 = (int) MessageBox.Show("Error outputting file", "Error");
          return false;
        }
        if (num2 != (ushort) 0 && num2 != (ushort) 1)
        {
          int num6 = (int) MessageBox.Show("Invalid midi type", "Error");
          return false;
        }
        int offset = 14;
        byte[] altPattern = (byte[]) null;
        byte altOffset = 0;
        byte altLength = 0;
        bool flag1 = false;
        uint num7 = 0;
        uint[] numArray3 = new uint[32];
        for (int index = 0; index < 32; ++index)
          numArray3[index] = 0U;
        for (int index3 = 0; index3 < (int) numTracks; ++index3)
        {
          uint num8 = 0;
          if (((((int) numArray2[offset] << 8 | (int) numArray2[offset + 1]) << 8 | (int) numArray2[offset + 2]) << 8 | (int) numArray2[offset + 3]) != 1297379947)
          {
            int num9 = (int) MessageBox.Show("Invalid track midi hdr", "Error");
            return false;
          }
          int num10 = (int) numArray2[offset + 4];
          int num11 = (int) numArray2[offset + 5];
          int num12 = (int) numArray2[offset + 6];
          int num13 = (int) numArray2[offset + 7];
          offset += 8;
          byte num14 = byte.MaxValue;
          bool flag2 = false;
          while (!flag2 && offset < int32_1)
          {
            uint original = 0;
            uint vlBytes = MidiParse.GetVLBytes(numArray2, ref offset, ref original, ref altPattern, ref altOffset, ref altLength, false);
            num8 += vlBytes;
            byte num15 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
            bool flag3 = num15 <= (byte) 127;
            if (num15 == byte.MaxValue)
            {
              byte num16 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              switch (num16)
              {
                case 47:
                  num8 -= vlBytes;
                  flag2 = true;
                  int num17 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  break;
                case 81:
                  int num18 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  int num19 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  int num20 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  int num21 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  break;
                default:
                  if (num16 < (byte) 127 && num16 != (byte) 81 && num16 != (byte) 47)
                  {
                    uint num22 = (uint) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                    for (int index4 = 0; (long) index4 < (long) num22; ++index4)
                    {
                      int num23 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                    }
                    break;
                  }
                  if (num16 == (byte) 127)
                  {
                    int int32_2 = Convert.ToInt32(MidiParse.GetVLBytes(numArray2, ref offset, ref original, ref altPattern, ref altOffset, ref altLength, false));
                    for (int index5 = 0; index5 < int32_2; ++index5)
                    {
                      int num24 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                    }
                    break;
                  }
                  break;
              }
              num14 = num15;
            }
            else if (num15 >= (byte) 128 && num15 < (byte) 144 || flag3 && num14 >= (byte) 128 && num14 < (byte) 144)
            {
              if (!flag3)
              {
                int num25 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              }
              int num26 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              if (!flag3)
                num14 = num15;
            }
            else if (num15 >= (byte) 144 && num15 < (byte) 160 || flag3 && num14 >= (byte) 144 && num14 < (byte) 160)
            {
              if (!flag3)
              {
                int num27 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              }
              int num28 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              if (!flag3)
                num14 = num15;
            }
            else if (num15 >= (byte) 176 && num15 < (byte) 192 || flag3 && num14 >= (byte) 176 && num14 < (byte) 192)
            {
              if (!flag3)
              {
                int num29 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              }
              int num30 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              if (!flag3)
                num14 = num15;
            }
            else if (num15 >= (byte) 192 && num15 < (byte) 208 || flag3 && num14 >= (byte) 192 && num14 < (byte) 208)
            {
              if (!flag3)
              {
                int num31 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              }
              if (!flag3)
                num14 = num15;
            }
            else if (num15 >= (byte) 208 && num15 < (byte) 224 || flag3 && num14 >= (byte) 208 && num14 < (byte) 224)
            {
              if (!flag3)
              {
                int num32 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              }
              if (!flag3)
                num14 = num15;
            }
            else if (num15 >= (byte) 224 && num15 < (byte) 240 || flag3 && num14 >= (byte) 224 && num14 < (byte) 240)
            {
              if (!flag3)
              {
                int num33 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              }
              int num34 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              if (!flag3)
                num14 = num15;
            }
            else if (num15 == (byte) 240 || num15 == (byte) 247)
            {
              int int32_3 = Convert.ToInt32(MidiParse.GetVLBytes(numArray2, ref offset, ref original, ref altPattern, ref altOffset, ref altLength, false));
              for (int index6 = 0; index6 < int32_3; ++index6)
              {
                int num35 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              }
            }
            else if (!flag1)
            {
              int num36 = (int) MessageBox.Show("Invalid midi character found", "Error");
              flag1 = true;
            }
          }
          if (num8 > num7)
            num7 = num8;
          if (num8 > numArray3[index3])
            numArray3[index3] = num8;
        }
        offset = 14;
        altPattern = (byte[]) null;
        altOffset = (byte) 0;
        altLength = (byte) 0;
        for (int index7 = 0; index7 < (int) numTracks; ++index7)
        {
          uint num37 = 0;
          if (((((int) numArray2[offset] << 8 | (int) numArray2[offset + 1]) << 8 | (int) numArray2[offset + 2]) << 8 | (int) numArray2[offset + 3]) != 1297379947)
          {
            int num38 = (int) MessageBox.Show("Invalid track midi hdr", "Error");
            return false;
          }
          int num39 = (int) numArray2[offset + 4];
          int num40 = (int) numArray2[offset + 5];
          int num41 = (int) numArray2[offset + 6];
          int num42 = (int) numArray2[offset + 7];
          offset += 8;
          byte num43 = byte.MaxValue;
          bool flag4 = false;
          bool flag5 = false;
          if (loop && loopPoint == 0U && numArray3[index7] > 0U)
          {
            TrackEvent trackEvent = trackEventArray[index7, numArray1[index7]];
            trackEvent.type = byte.MaxValue;
            trackEvent.absoluteTime = 0U;
            trackEvent.contentSize = 3;
            trackEvent.contents = new byte[trackEvent.contentSize];
            trackEvent.contents[0] = (byte) 46;
            trackEvent.contents[1] = (byte) 0;
            trackEvent.contents[2] = byte.MaxValue;
            trackEvent.deltaTime = 0U;
            trackEvent.obsoleteEvent = false;
            ++numArray1[index7];
            flag5 = true;
          }
          while (!flag4 && offset < int32_1)
          {
            uint original = 0;
            uint vlBytes = MidiParse.GetVLBytes(numArray2, ref offset, ref original, ref altPattern, ref altOffset, ref altLength, false);
            num37 += vlBytes;
            TrackEvent trackEvent1 = trackEventArray[index7, numArray1[index7]];
            trackEvent1.deltaTime = vlBytes;
            trackEvent1.obsoleteEvent = false;
            trackEvent1.contents = (byte[]) null;
            trackEvent1.absoluteTime = num37;
            if (loop && !flag5 && numArray3[index7] > loopPoint)
            {
              if ((int) num37 == (int) loopPoint)
              {
                TrackEvent trackEvent2 = trackEventArray[index7, numArray1[index7]];
                trackEvent2.type = byte.MaxValue;
                trackEvent2.absoluteTime = num37;
                trackEvent2.contentSize = 3;
                trackEvent2.contents = new byte[trackEvent2.contentSize];
                trackEvent2.contents[0] = (byte) 46;
                trackEvent2.contents[1] = (byte) 0;
                trackEvent2.contents[2] = byte.MaxValue;
                trackEvent2.deltaTime = vlBytes;
                trackEvent2.obsoleteEvent = false;
                ++numArray1[index7];
                trackEvent1 = trackEventArray[index7, numArray1[index7]];
                trackEvent1.deltaTime = 0U;
                trackEvent1.obsoleteEvent = false;
                trackEvent1.contents = (byte[]) null;
                trackEvent1.absoluteTime = num37;
                flag5 = true;
              }
              else if (num37 > loopPoint)
              {
                TrackEvent trackEvent3 = trackEventArray[index7, numArray1[index7]];
                trackEvent3.type = byte.MaxValue;
                trackEvent3.absoluteTime = loopPoint;
                trackEvent3.contentSize = 3;
                trackEvent3.contents = new byte[trackEvent3.contentSize];
                trackEvent3.contents[0] = (byte) 46;
                trackEvent3.contents[1] = (byte) 0;
                trackEvent3.contents[2] = byte.MaxValue;
                trackEvent3.deltaTime = numArray1[index7] <= 0 ? loopPoint : loopPoint - trackEventArray[index7, numArray1[index7] - 1].absoluteTime;
                trackEvent3.obsoleteEvent = false;
                ++numArray1[index7];
                trackEvent1 = trackEventArray[index7, numArray1[index7]];
                trackEvent1.deltaTime = num37 - loopPoint;
                trackEvent1.obsoleteEvent = false;
                trackEvent1.contents = (byte[]) null;
                trackEvent1.absoluteTime = num37;
                flag5 = true;
              }
            }
            byte num44 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
            bool flag6 = num44 <= (byte) 127;
            if (num44 == byte.MaxValue)
            {
              byte num45 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              switch (num45)
              {
                case 47:
                  flag4 = true;
                  if (loop && numArray3[index7] > loopPoint)
                  {
                    TrackEvent trackEvent4 = trackEventArray[index7, numArray1[index7] - 1];
                    if (trackEvent4.type == byte.MaxValue && trackEvent4.contentSize > 0 && trackEvent4.contents[0] == (byte) 46)
                    {
                      TrackEvent trackEvent5 = trackEvent4;
                      trackEvent5.type = byte.MaxValue;
                      trackEvent5.contentSize = 1;
                      trackEvent5.contents = new byte[trackEvent5.contentSize];
                      trackEvent5.contents[0] = (byte) 47;
                    }
                    else
                    {
                      TrackEvent trackEvent6 = trackEventArray[index7, numArray1[index7] + 1];
                      trackEvent6.absoluteTime = num7;
                      trackEvent6.deltaTime = 0U;
                      trackEvent6.durationTime = trackEvent1.durationTime;
                      trackEvent6.obsoleteEvent = trackEvent1.obsoleteEvent;
                      trackEvent6.type = byte.MaxValue;
                      trackEvent6.contentSize = 1;
                      trackEvent6.contents = new byte[trackEvent6.contentSize];
                      trackEvent6.contents[0] = (byte) 47;
                      trackEvent1.type = byte.MaxValue;
                      if (num7 > trackEvent4.absoluteTime + trackEvent4.durationTime)
                      {
                        trackEvent1.deltaTime = num7 - (trackEvent4.absoluteTime + trackEvent4.durationTime);
                        trackEvent1.absoluteTime = num7;
                      }
                      else
                      {
                        trackEvent1.deltaTime = 0U;
                        trackEvent1.absoluteTime = trackEvent4.absoluteTime;
                      }
                      trackEvent1.contentSize = 7;
                      trackEvent1.contents = new byte[trackEvent1.contentSize];
                      trackEvent1.contents[0] = (byte) 45;
                      trackEvent1.contents[1] = byte.MaxValue;
                      trackEvent1.contents[2] = byte.MaxValue;
                      trackEvent1.contents[3] = (byte) 0;
                      trackEvent1.contents[4] = (byte) 0;
                      trackEvent1.contents[5] = (byte) 0;
                      trackEvent1.contents[6] = (byte) 0;
                      trackEvent1.obsoleteEvent = false;
                      ++numArray1[index7];
                    }
                  }
                  else
                  {
                    trackEvent1.type = byte.MaxValue;
                    trackEvent1.contentSize = 1;
                    trackEvent1.contents = new byte[trackEvent1.contentSize];
                    trackEvent1.contents[0] = (byte) 47;
                  }
                  int num46 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  break;
                case 81:
                  trackEvent1.type = byte.MaxValue;
                  trackEvent1.contentSize = 4;
                  trackEvent1.contents = new byte[trackEvent1.contentSize];
                  trackEvent1.contents[0] = (byte) 81;
                  int num47 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  trackEvent1.contents[1] = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  trackEvent1.contents[2] = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  trackEvent1.contents[3] = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                  break;
                default:
                  if (num45 < (byte) 127 && num45 != (byte) 81 && num45 != (byte) 47)
                  {
                    trackEvent1.type = byte.MaxValue;
                    uint num48 = (uint) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                    for (int index8 = 0; (long) index8 < (long) num48; ++index8)
                    {
                      int num49 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                    }
                    trackEvent1.obsoleteEvent = true;
                    break;
                  }
                  if (num45 == (byte) 127)
                  {
                    trackEvent1.type = byte.MaxValue;
                    int int32_4 = Convert.ToInt32(MidiParse.GetVLBytes(numArray2, ref offset, ref original, ref altPattern, ref altOffset, ref altLength, false));
                    for (int index9 = 0; index9 < int32_4; ++index9)
                    {
                      int num50 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                    }
                    trackEvent1.obsoleteEvent = true;
                    break;
                  }
                  break;
              }
              num43 = num44;
            }
            else if (num44 >= (byte) 128 && num44 < (byte) 144 || flag6 && num43 >= (byte) 128 && num43 < (byte) 144)
            {
              byte num51;
              byte num52;
              if (flag6)
              {
                trackEvent1.type = num43;
                num51 = num44;
                num52 = num43;
              }
              else
              {
                trackEvent1.type = num44;
                num51 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                num52 = num44;
              }
              byte num53 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              for (int index10 = numArray1[index7] - 1; index10 >= 0; --index10)
              {
                if ((int) trackEventArray[index7, index10].type == (144 | (int) num52 & 15) && !trackEventArray[index7, index10].obsoleteEvent && (int) trackEventArray[index7, index10].contents[0] == (int) num51)
                {
                  trackEventArray[index7, index10].durationTime = num37 - trackEventArray[index7, index10].absoluteTime;
                  break;
                }
              }
              trackEvent1.durationTime = 0U;
              trackEvent1.contentSize = 2;
              trackEvent1.contents = new byte[trackEvent1.contentSize];
              trackEvent1.contents[0] = num51;
              trackEvent1.contents[1] = num53;
              trackEvent1.obsoleteEvent = true;
              if (!flag6)
                num43 = num44;
            }
            else if (num44 >= (byte) 144 && num44 < (byte) 160 || flag6 && num43 >= (byte) 144 && num43 < (byte) 160)
            {
              byte num54;
              byte num55;
              if (flag6)
              {
                trackEvent1.type = num43;
                num54 = num44;
                num55 = num43;
              }
              else
              {
                trackEvent1.type = num44;
                num54 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                num55 = num44;
              }
              byte num56 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              if (num56 == (byte) 0)
              {
                for (int index11 = numArray1[index7] - 1; index11 >= 0; --index11)
                {
                  if ((int) trackEventArray[index7, index11].type == (int) num55 && !trackEventArray[index7, index11].obsoleteEvent && (int) trackEventArray[index7, index11].contents[0] == (int) num54)
                  {
                    trackEventArray[index7, index11].durationTime = num37 - trackEventArray[index7, index11].absoluteTime;
                    break;
                  }
                }
                trackEvent1.durationTime = 0U;
                trackEvent1.contentSize = 2;
                trackEvent1.contents = new byte[trackEvent1.contentSize];
                trackEvent1.contents[0] = num54;
                trackEvent1.contents[1] = num56;
                trackEvent1.obsoleteEvent = true;
              }
              else
              {
                for (int index12 = numArray1[index7] - 1; index12 >= 0; --index12)
                {
                  if ((int) trackEventArray[index7, index12].type == (int) num55 && !trackEventArray[index7, index12].obsoleteEvent && (int) trackEventArray[index7, index12].contents[0] == (int) num54)
                  {
                    if (trackEventArray[index7, index12].durationTime == 0U)
                    {
                      trackEventArray[index7, index12].durationTime = num37 - trackEventArray[index7, index12].absoluteTime;
                      break;
                    }
                    break;
                  }
                }
                trackEvent1.durationTime = 0U;
                trackEvent1.contentSize = 2;
                trackEvent1.contents = new byte[trackEvent1.contentSize];
                trackEvent1.contents[0] = num54;
                trackEvent1.contents[1] = num56;
              }
              if (!flag6)
                num43 = num44;
            }
            else if (num44 >= (byte) 176 && num44 < (byte) 192 || flag6 && num43 >= (byte) 176 && num43 < (byte) 192)
            {
              byte num57;
              if (flag6)
              {
                num57 = num44;
                trackEvent1.type = num43;
              }
              else
              {
                num57 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                trackEvent1.type = num44;
              }
              byte num58 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              trackEvent1.contentSize = 2;
              trackEvent1.contents = new byte[trackEvent1.contentSize];
              trackEvent1.contents[0] = num57;
              trackEvent1.contents[1] = num58;
              if (!flag6)
                num43 = num44;
            }
            else if (num44 >= (byte) 192 && num44 < (byte) 208 || flag6 && num43 >= (byte) 192 && num43 < (byte) 208)
            {
              byte num59;
              if (flag6)
              {
                num59 = num44;
                trackEvent1.type = num43;
              }
              else
              {
                num59 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                trackEvent1.type = num44;
              }
              trackEvent1.contentSize = 1;
              trackEvent1.contents = new byte[trackEvent1.contentSize];
              trackEvent1.contents[0] = num59;
              if (!flag6)
                num43 = num44;
            }
            else if (num44 >= (byte) 208 && num44 < (byte) 224 || flag6 && num43 >= (byte) 208 && num43 < (byte) 224)
            {
              trackEvent1.type = num44;
              byte num60;
              if (flag6)
              {
                num60 = num44;
                trackEvent1.type = num43;
              }
              else
              {
                num60 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                trackEvent1.type = num44;
              }
              trackEvent1.contentSize = 1;
              trackEvent1.contents = new byte[trackEvent1.contentSize];
              trackEvent1.contents[0] = num60;
              if (!flag6)
                num43 = num44;
            }
            else if (num44 >= (byte) 224 && num44 < (byte) 240 || flag6 && num43 >= (byte) 224 && num43 < (byte) 240)
            {
              trackEvent1.type = num44;
              byte num61;
              if (flag6)
              {
                num61 = num44;
                trackEvent1.type = num43;
              }
              else
              {
                num61 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
                trackEvent1.type = num44;
              }
              byte num62 = MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              trackEvent1.contentSize = 2;
              trackEvent1.contents = new byte[trackEvent1.contentSize];
              trackEvent1.contents[0] = num61;
              trackEvent1.contents[1] = num62;
              if (!flag6)
                num43 = num44;
            }
            else if (num44 == (byte) 240 || num44 == (byte) 247)
            {
              trackEvent1.type = num44;
              int int32_5 = Convert.ToInt32(MidiParse.GetVLBytes(numArray2, ref offset, ref original, ref altPattern, ref altOffset, ref altLength, false));
              for (int index13 = 0; index13 < int32_5; ++index13)
              {
                int num63 = (int) MidiParse.ReadMidiByte(numArray2, ref offset, ref altPattern, ref altOffset, ref altLength, false);
              }
              trackEvent1.obsoleteEvent = true;
            }
            else if (!flag1)
            {
              int num64 = (int) MessageBox.Show("Invalid midi character found", "Error");
              flag1 = true;
            }
            ++numArray1[index7];
          }
        }
        uint num65 = 0;
        uint inLong = 132;
        for (int index14 = 0; index14 < (int) numTracks; ++index14)
        {
          uint num66 = 0;
          int num67 = 0;
          byte num68 = 0;
          if (numArray1[index14] > 0)
          {
            uint num69 = MidiParse.Flip32Bit(inLong);
            outFile.Write(BitConverter.GetBytes(num69), 0, 4);
            for (int index15 = 0; index15 < numArray1[index14]; ++index15)
            {
              TrackEvent trackEvent = trackEventArray[index14, index15];
              uint length1 = 0;
              int num70 = (int) MidiParse.ReturnVLBytes(trackEvent.deltaTime + num65, ref length1);
              if (trackEvent.obsoleteEvent)
              {
                num65 += trackEvent.deltaTime;
              }
              else
              {
                if (trackEvent.type == byte.MaxValue && trackEvent.contents[0] == (byte) 46)
                  num67 = Convert.ToInt32((long) (uint) ((int) inLong + (int) num66 + 1) + (long) trackEvent.contentSize + (long) length1);
                num65 = 0U;
                uint num71 = num66 + length1;
                if (trackEvent.type == byte.MaxValue && trackEvent.contents[0] == (byte) 45)
                {
                  uint uint32_2 = Convert.ToUInt32((long) (inLong + num71) - (long) num67 + 8L);
                  trackEvent.contents[3] = Convert.ToByte(uint32_2 >> 24 & (uint) byte.MaxValue);
                  trackEvent.contents[4] = Convert.ToByte(uint32_2 >> 16 & (uint) byte.MaxValue);
                  trackEvent.contents[5] = Convert.ToByte(uint32_2 >> 8 & (uint) byte.MaxValue);
                  trackEvent.contents[6] = Convert.ToByte(uint32_2 & (uint) byte.MaxValue);
                }
                if ((int) trackEvent.type != (int) num68 || trackEvent.type == byte.MaxValue)
                  ++num71;
                num66 = num71 + Convert.ToUInt32(trackEvent.contentSize);
                if (trackEvent.type >= (byte) 144 && trackEvent.type < (byte) 160)
                {
                  uint length2 = 0;
                  int num72 = (int) MidiParse.ReturnVLBytes(trackEvent.durationTime, ref length2);
                  num66 += length2;
                }
                num68 = trackEvent.type;
              }
            }
            inLong += num66;
          }
          else
          {
            uint num73 = 0;
            outFile.Write(BitConverter.GetBytes(num73), 0, 4);
          }
        }
        for (int index = (int) numTracks; index < 32; ++index)
        {
          uint num74 = 0;
          outFile.Write(BitConverter.GetBytes(num74), 0, 4);
        }
        uint num75 = MidiParse.Flip32Bit((uint) num3);
        outFile.Write(BitConverter.GetBytes(num75), 0, 4);
        for (int index16 = 0; index16 < (int) numTracks; ++index16)
        {
          if (numArray1[index16] > 0)
          {
            byte num76 = 0;
            for (int index17 = 0; index17 < numArray1[index16]; ++index17)
            {
              TrackEvent trackEvent = trackEventArray[index16, index17];
              if (trackEvent.obsoleteEvent)
              {
                num65 += trackEvent.deltaTime;
              }
              else
              {
                uint length3 = 0;
                uint num77 = MidiParse.ReturnVLBytes(trackEvent.deltaTime + num65, ref length3);
                num65 = 0U;
                MidiParse.WriteVLBytes(outFile, num77, length3, false);
                if ((int) trackEvent.type != (int) num76 || trackEvent.type == byte.MaxValue)
                  outFile.WriteByte(trackEvent.type);
                outFile.Write(trackEvent.contents, 0, trackEvent.contentSize);
                if (trackEvent.type >= (byte) 144 && trackEvent.type < (byte) 160)
                {
                  uint length4 = 0;
                  uint num78 = MidiParse.ReturnVLBytes(trackEvent.durationTime, ref length4);
                  MidiParse.WriteVLBytes(outFile, num78, length4, false);
                }
                num76 = trackEvent.type;
              }
            }
          }
          for (int index18 = 0; index18 < numArray1[index16]; ++index18)
          {
            if (trackEventArray[index16, index18].contents != null)
              trackEventArray[index16, index18].contents = (byte[]) null;
          }
        }
        outFile.Close();
        outFile.Dispose();
        int int32_6 = Convert.ToInt32(new FileInfo(output).Length);
        byte[] Buffer1 = File.ReadAllBytes(output);
        uint[] numArray4 = new uint[32];
        int[] numArray5 = new int[32];
        for (int index = 0; index < 128; index += 4)
        {
          numArray4[index / 4] = (uint) ((((int) Buffer1[index] << 8 | (int) Buffer1[index + 1]) << 8 | (int) Buffer1[index + 2]) << 8) | (uint) Buffer1[index + 3];
          numArray5[index / 4] = 0;
        }
        for (int index19 = 0; index19 < int32_6; ++index19)
        {
          if (index19 > 132 && Buffer1[index19] == (byte) 254)
          {
            for (int index20 = 0; index20 < (int) numTracks; ++index20)
            {
              if ((long) numArray4[index20] > (long) index19)
                ++numArray5[index20];
            }
          }
        }
        FileStream fileStream1 = new FileStream(output, FileMode.Create, FileAccess.Write);
        for (int index = 0; index < 32; ++index)
          MidiParse.WriteLongToBuffer(Buffer1, Convert.ToUInt32(index * 4), numArray4[index] + Convert.ToUInt32(numArray5[index]));
        for (int index = 0; index < int32_6; ++index)
        {
          fileStream1.WriteByte(Buffer1[index]);
          if (index > 132 && Buffer1[index] == (byte) 254)
            fileStream1.WriteByte(Buffer1[index]);
        }
        fileStream1.Close();
        fileStream1.Dispose();
        byte[] numArray6 = (byte[]) null;
        if (useRepeaters)
        {
          int int32_7 = Convert.ToInt32(new FileInfo(output).Length);
          byte[] numArray7 = File.ReadAllBytes(output);
          uint[] numArray8 = new uint[32];
          for (int index = 0; index < 128; index += 4)
            numArray8[index / 4] = (uint) ((((int) numArray7[index] << 8 | (int) numArray7[index + 1]) << 8 | (int) numArray7[index + 2]) << 8) | (uint) numArray7[index + 3];
          uint data = (uint) ((((int) numArray7[128] << 8 | (int) numArray7[129]) << 8 | (int) numArray7[130]) << 8) | (uint) numArray7[131];
          byte[] Buffer2 = new byte[int32_7 * 4];
          for (int index = 0; index < int32_7 * 4; ++index)
            Buffer2[index] = (byte) 0;
          uint[] numArray9 = new uint[32];
          for (int index = 0; index < 32; ++index)
            numArray9[index] = 0U;
          int num79 = 132;
          for (int index21 = 0; index21 < 32 && numArray8[index21] != 0U; ++index21)
          {
            numArray9[index21] = Convert.ToUInt32(num79);
            int num80 = num79;
            int num81 = int32_7;
            if (index21 < 15 && numArray8[index21 + 1] != 0U)
              num81 = Convert.ToInt32(numArray8[index21 + 1]);
            int int32_8 = Convert.ToInt32(numArray8[index21]);
            while (int32_8 < num81)
            {
              int num82 = -1;
              int num83 = -1;
              for (int index22 = num80; index22 < num79; ++index22)
              {
                int num84;
                for (num84 = 0; (int) Buffer2[index22 + num84] == (int) numArray7[int32_8 + num84] && int32_8 + num84 < num81 && Buffer2[index22 + num84] != (byte) 254 && Buffer2[index22 + num84] != byte.MaxValue && index22 + num84 < num79; ++num84)
                {
                  bool flag7 = false;
                  for (int index23 = int32_8 + num84; index23 < num81 && index23 < int32_8 + num84 + 5; ++index23)
                  {
                    if (numArray7[index23] == byte.MaxValue)
                      flag7 = true;
                  }
                  if (flag7)
                    break;
                }
                if (num84 > num83 && num84 > 6)
                {
                  num83 = num84;
                  num82 = index22;
                }
              }
              Convert.ToInt32(((long) int32_8 - (long) numArray8[index21]) / 2L);
              if (num83 > 6)
              {
                if (num83 > 253)
                  num83 = 253;
                byte[] numArray10 = Buffer2;
                int index24 = num79;
                int num85 = index24 + 1;
                numArray10[index24] = (byte) 254;
                int num86 = num85 - num82 - 1;
                byte[] numArray11 = Buffer2;
                int index25 = num85;
                int num87 = index25 + 1;
                int num88 = (int) Convert.ToByte(num86 >> 8 & (int) byte.MaxValue);
                numArray11[index25] = (byte) num88;
                byte[] numArray12 = Buffer2;
                int index26 = num87;
                int num89 = index26 + 1;
                int num90 = (int) Convert.ToByte(num86 & (int) byte.MaxValue);
                numArray12[index26] = (byte) num90;
                byte[] numArray13 = Buffer2;
                int index27 = num89;
                num79 = index27 + 1;
                int num91 = (int) Convert.ToByte(num83);
                numArray13[index27] = (byte) num91;
                int32_8 += num83;
              }
              else
              {
                Buffer2[num79++] = numArray7[int32_8];
                ++int32_8;
              }
            }
            if (num79 % 4 != 0)
              num79 += 4 - num79 % 4;
          }
          for (int index = 0; index < 32; ++index)
          {
            if (numArray9[index] != 0U)
            {
              Convert.ToInt32(numArray9[index]);
              int num92 = num79;
              if (index < 15 && numArray9[index + 1] != 0U)
                num92 = Convert.ToInt32(numArray9[index + 1]);
              int int32_9 = Convert.ToInt32(numArray9[index]);
              bool flag8 = false;
              int num93 = 0;
              while (int32_9 < num92)
              {
                if (Buffer2[int32_9] == byte.MaxValue && Buffer2[int32_9 + 1] == (byte) 46 && Buffer2[int32_9 + 2] == (byte) 0 && Buffer2[int32_9 + 3] == byte.MaxValue)
                {
                  flag8 = true;
                  num93 = int32_9 + 4;
                  int32_9 += 4;
                }
                else if (Buffer2[int32_9] == byte.MaxValue && Buffer2[int32_9 + 1] == (byte) 45 && Buffer2[int32_9 + 2] == byte.MaxValue && Buffer2[int32_9 + 3] == byte.MaxValue)
                {
                  if (flag8)
                  {
                    int num94 = int32_9 + 8 - num93;
                    MidiParse.WriteLongToBuffer(Buffer2, Convert.ToUInt32(int32_9 + 4), Convert.ToUInt32(num94));
                    flag8 = false;
                  }
                  int32_9 += 8;
                }
                else
                  ++int32_9;
              }
            }
          }
          for (int index = 0; index < 32; ++index)
            MidiParse.WriteLongToBuffer(Buffer2, Convert.ToUInt32(index * 4), Convert.ToUInt32(numArray9[index]));
          MidiParse.WriteLongToBuffer(Buffer2, 128U, data);
          FileStream fileStream2 = new FileStream(output, FileMode.Create, FileAccess.Write);
          for (int index = 0; index < num79; ++index)
            fileStream2.WriteByte(Buffer2[index]);
          fileStream2.Close();
          fileStream2.Dispose();
          numArray6 = (byte[]) null;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error converting " + ex.ToString(), "Error");
        return false;
      }
      return true;
    }
  }
}
