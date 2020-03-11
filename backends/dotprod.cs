using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using static System.ValueTuple;
using static System.Convert;
using static System.Math;
using System.Numerics;
using Mono.Options;
namespace FutharkInternal
{
    public class FutharkInternal
    {
        FileStream RuntimeFile;
        StreamWriter RuntimeFileWriter;
        bool DoWarmupRun;
        int NumRuns;
        string EntryPoint;
        public FutharkInternal(string[] args)
        {
            DoWarmupRun = false;
            NumRuns = 1;
            EntryPoint = "main";
            ValueReader();
            var options = new OptionSet{{"t|write-runtime-to=",
                                         optarg => {if ((RuntimeFile != null))
                                                    {
                                                        RuntimeFile.Close();
                                                    }
                                                    RuntimeFile = new FileStream(optarg,
                                                                                 FileMode.Create);
                                                    RuntimeFileWriter = new StreamWriter(RuntimeFile);}},
                                        {"r|runs=",
                                         optarg => {NumRuns = Convert.ToInt32(optarg);
                                                    DoWarmupRun = true;}},
                                        {"e|entry-point=",
                                         optarg => {EntryPoint = optarg;}}};
            var extra = options.Parse(args);
        }
        // Scalar functions.
        private static sbyte signed(byte x){ return (sbyte) x;}
        private static short signed(ushort x){ return (short) x;}
        private static int signed(uint x){ return (int) x;}
        private static long signed(ulong x){ return (long) x;}
        
        private static byte unsigned(sbyte x){ return (byte) x;}
        private static ushort unsigned(short x){ return (ushort) x;}
        private static uint unsigned(int x){ return (uint) x;}
        private static ulong unsigned(long x){ return (ulong) x;}
        
        private static sbyte add8(sbyte x, sbyte y){ return (sbyte) ((byte) x + (byte) y);}
        private static short add16(short x, short y){ return (short) ((ushort) x + (ushort) y);}
        private static int add32(int x, int y){ return (int) ((uint) x + (uint) y);}
        private static long add64(long x, long y){ return (long) ((ulong) x + (ulong) y);}
        
        private static sbyte sub8(sbyte x, sbyte y){ return (sbyte) ((byte) x - (byte) y);}
        private static short sub16(short x, short y){ return (short) ((ushort) x - (ushort) y);}
        private static int sub32(int x, int y){ return (int) ((uint) x - (uint) y);}
        private static long sub64(long x, long y){ return (long) ((ulong) x - (ulong) y);}
        
        private static sbyte mul8(sbyte x, sbyte y){ return (sbyte) ((byte) x * (byte) y);}
        private static short mul16(short x, short y){ return (short) ((ushort) x * (ushort) y);}
        private static int mul32(int x, int y){ return (int) ((uint) x * (uint) y);}
        private static long mul64(long x, long y){ return (long) ((ulong) x * (ulong) y);}
        
        private static sbyte or8(sbyte x, sbyte y){ return (sbyte) (x | y); }
        private static short or16(short x, short y){ return (short) (x | y); }
        private static int or32(int x, int y){ return x | y; }
        private static long or64(long x, long y){ return x | y;}
        
        private static sbyte xor8(sbyte x, sbyte y){ return (sbyte) (x ^ y); }
        private static short xor16(short x, short y){ return (short) (x ^ y); }
        private static int xor32(int x, int y){ return x ^ y; }
        private static long xor64(long x, long y){ return x ^ y;}
        
        private static sbyte and8(sbyte x, sbyte y){ return (sbyte) (x & y); }
        private static short and16(short x, short y){ return (short) (x & y); }
        private static int and32(int x, int y){ return x & y; }
        private static long and64(long x, long y){ return x & y;}
        
        private static sbyte shl8(sbyte x, sbyte y){ return (sbyte) (x << y); }
        private static short shl16(short x, short y){ return (short) (x << y); }
        private static int shl32(int x, int y){ return x << y; }
        private static long shl64(long x, long y){ return x << Convert.ToInt32(y); }
        
        private static sbyte ashr8(sbyte x, sbyte y){ return (sbyte) (x >> y); }
        private static short ashr16(short x, short y){ return (short) (x >> y); }
        private static int ashr32(int x, int y){ return x >> y; }
        private static long ashr64(long x, long y){ return x >> Convert.ToInt32(y); }
        
        private static sbyte sdiv8(sbyte x, sbyte y){
            var q = squot8(x,y);
            var r = srem8(x,y);
            return (sbyte) (q - (((r != (sbyte) 0) && ((r < (sbyte) 0) != (y < (sbyte) 0))) ? (sbyte) 1 : (sbyte) 0));
        }
        private static short sdiv16(short x, short y){
            var q = squot16(x,y);
            var r = srem16(x,y);
            return (short) (q - (((r != (short) 0) && ((r < (short) 0) != (y < (short) 0))) ? (short) 1 : (short) 0));
        }
        private static int sdiv32(int x, int y){
            var q = squot32(x,y);
            var r = srem32(x,y);
            return q - (((r != (int) 0) && ((r < (int) 0) != (y < (int) 0))) ? (int) 1 : (int) 0);
        }
        private static long sdiv64(long x, long y){
            var q = squot64(x,y);
            var r = srem64(x,y);
            return q - (((r != (long) 0) && ((r < (long) 0) != (y < (long) 0))) ? (long) 1 : (long) 0);
        }
        
        private static sbyte smod8(sbyte x, sbyte y){
            var r = srem8(x,y);
            return (sbyte) (r + ((r == (sbyte) 0 || (x > (sbyte) 0 && y > (sbyte) 0) || (x < (sbyte) 0 && y < (sbyte) 0)) ? (sbyte) 0 : y));
        }
        private static short smod16(short x, short y){
            var r = srem16(x,y);
            return (short) (r + ((r == (short) 0 || (x > (short) 0 && y > (short) 0) || (x < (short) 0 && y < (short) 0)) ? (short) 0 : y));
        }
        private static int smod32(int x, int y){
            var r = srem32(x,y);
            return (int) r + ((r == (int) 0 || (x > (int) 0 && y > (int) 0) || (x < (int) 0 && y < (int) 0)) ? (int) 0 : y);
        }
        private static long smod64(long x, long y){
            var r = srem64(x,y);
            return (long) r + ((r == (long) 0 || (x > (long) 0 && y > (long) 0) || (x < (long) 0 && y < (long) 0)) ? (long) 0 : y);
        }
        
        private static sbyte udiv8(sbyte x, sbyte y){ return signed((byte) (unsigned(x) / unsigned(y))); }
        private static short udiv16(short x, short y){ return signed((ushort) (unsigned(x) / unsigned(y))); }
        private static int udiv32(int x, int y){ return signed(unsigned(x) / unsigned(y)); }
        private static long udiv64(long x, long y){ return signed(unsigned(x) / unsigned(y)); }
        
        private static sbyte umod8(sbyte x, sbyte y){ return signed((byte) (unsigned(x) % unsigned(y))); }
        private static short umod16(short x, short y){ return signed((ushort) (unsigned(x) % unsigned(y))); }
        private static int umod32(int x, int y){ return signed(unsigned(x) % unsigned(y)); }
        private static long umod64(long x, long y){ return signed(unsigned(x) % unsigned(y)); }
        
        private static sbyte squot8(sbyte x, sbyte y){ return (sbyte) Math.Truncate(ToSingle(x) / ToSingle(y)); }
        private static short squot16(short x, short y){ return (short) Math.Truncate(ToSingle(x) / ToSingle(y)); }
        private static int squot32(int x, int y){ return (int) Math.Truncate(ToSingle(x) / ToSingle(y)); }
        private static long squot64(long x, long y){ return (long) Math.Truncate(ToSingle(x) / ToSingle(y)); }
        
        // private static Maybe change srem, it calls np.fmod originally so i dont know
        private static sbyte srem8(sbyte x, sbyte y){ return (sbyte) ((sbyte) x % (sbyte) y);}
        private static short srem16(short x, short y){ return (short) ((short) x % (short) y);}
        private static int srem32(int x, int y){ return (int) ((int) x % (int) y);}
        private static long srem64(long x, long y){ return (long) ((long) x % (long) y);}
        
        private static sbyte smin8(sbyte x, sbyte y){ return Math.Min(x,y);}
        private static short smin16(short x, short y){ return Math.Min(x,y);}
        private static int smin32(int x, int y){ return Math.Min(x,y);}
        private static long smin64(long x, long y){ return Math.Min(x,y);}
        
        private static sbyte smax8(sbyte x, sbyte y){ return Math.Max(x,y);}
        private static short smax16(short x, short y){ return Math.Max(x,y);}
        private static int smax32(int x, int y){ return Math.Max(x,y);}
        private static long smax64(long x, long y){ return Math.Max(x,y);}
        
        private static sbyte umin8(sbyte x, sbyte y){ return signed(Math.Min(unsigned(x),unsigned(y)));}
        private static short umin16(short x, short y){ return signed(Math.Min(unsigned(x),unsigned(y)));}
        private static int umin32(int x, int y){ return signed(Math.Min(unsigned(x),unsigned(y)));}
        private static long umin64(long x, long y){ return signed(Math.Min(unsigned(x),unsigned(y)));}
        
        private static sbyte umax8(sbyte x, sbyte y){ return signed(Math.Max(unsigned(x),unsigned(y)));}
        private static short umax16(short x, short y){ return signed(Math.Max(unsigned(x),unsigned(y)));}
        private static int umax32(int x, int y){ return signed(Math.Max(unsigned(x),unsigned(y)));}
        private static long umax64(long x, long y){ return signed(Math.Max(unsigned(x),unsigned(y)));}
        
        private static float fmin32(float x, float y){ return Math.Min(x,y);}
        private static double fmin64(double x, double y){ return Math.Min(x,y);}
        private static float fmax32(float x, float y){ return Math.Max(x,y);}
        private static double fmax64(double x, double y){ return Math.Max(x,y);}
        
        private static sbyte pow8(sbyte x, sbyte y){sbyte res = 1;for (var i = 0; i < y; i++){res *= x;}return res;}
        private static short pow16(short x, short y){short res = 1;for (var i = 0; i < y; i++){res *= x;}return res;}
        private static int pow32(int x, int y){int res = 1;for (var i = 0; i < y; i++){res *= x;}return res;}
        private static long pow64(long x, long y){long res = 1;for (var i = 0; i < y; i++){res *= x;}return res;}
        
        private static float fpow32(float x, float y){ return Convert.ToSingle(Math.Pow(x,y));}
        private static double fpow64(double x, double y){ return Convert.ToDouble(Math.Pow(x,y));}
        
        private static bool sle8(sbyte x, sbyte y){ return x <= y ;}
        private static bool sle16(short x, short y){ return x <= y ;}
        private static bool sle32(int x, int y){ return x <= y ;}
        private static bool sle64(long x, long y){ return x <= y ;}
        
        private static bool slt8(sbyte x, sbyte y){ return x < y ;}
        private static bool slt16(short x, short y){ return x < y ;}
        private static bool slt32(int x, int y){ return x < y ;}
        private static bool slt64(long x, long y){ return x < y ;}
        
        private static bool ule8(sbyte x, sbyte y){ return unsigned(x) <= unsigned(y) ;}
        private static bool ule16(short x, short y){ return unsigned(x) <= unsigned(y) ;}
        private static bool ule32(int x, int y){ return unsigned(x) <= unsigned(y) ;}
        private static bool ule64(long x, long y){ return unsigned(x) <= unsigned(y) ;}
        
        private static bool ult8(sbyte x, sbyte y){ return unsigned(x) < unsigned(y) ;}
        private static bool ult16(short x, short y){ return unsigned(x) < unsigned(y) ;}
        private static bool ult32(int x, int y){ return unsigned(x) < unsigned(y) ;}
        private static bool ult64(long x, long y){ return unsigned(x) < unsigned(y) ;}
        
        private static sbyte lshr8(sbyte x, sbyte y){ return (sbyte) ((uint) x >> (int) y);}
        private static short lshr16(short x, short y){ return (short) ((ushort) x >> (int) y);}
        private static int lshr32(int x, int y){ return (int) ((uint) (x) >> (int) y);}
        private static long lshr64(long x, long y){ return (long) ((ulong) x >> (int) y);}
        
        private static sbyte sext_i8_i8(sbyte x){return (sbyte) (x);}
        private static short sext_i8_i16(sbyte x){return (short) (x);}
        private static int sext_i8_i32(sbyte x){return (int) (x);}
        private static long sext_i8_i64(sbyte x){return (long) (x);}
        
        private static sbyte sext_i16_i8(short x){return (sbyte) (x);}
        private static short sext_i16_i16(short x){return (short) (x);}
        private static int sext_i16_i32(short x){return (int) (x);}
        private static long sext_i16_i64(short x){return (long) (x);}
        
        private static sbyte sext_i32_i8(int x){return (sbyte) (x);}
        private static short sext_i32_i16(int x){return (short) (x);}
        private static int sext_i32_i32(int x){return (int) (x);}
        private static long sext_i32_i64(int x){return (long) (x);}
        
        private static sbyte sext_i64_i8(long x){return (sbyte) (x);}
        private static short sext_i64_i16(long x){return (short) (x);}
        private static int sext_i64_i32(long x){return (int) (x);}
        private static long sext_i64_i64(long x){return (long) (x);}
        
        private static sbyte btoi_bool_i8 (bool x){return (sbyte) (Convert.ToInt32(x));}
        private static short btoi_bool_i16(bool x){return (short) (Convert.ToInt32(x));}
        private static int   btoi_bool_i32(bool x){return (int)   (Convert.ToInt32(x));}
        private static long  btoi_bool_i64(bool x){return (long)  (Convert.ToInt32(x));}
        
        private static bool itob_i8_bool (sbyte x){return x != 0;}
        private static bool itob_i16_bool(short x){return x != 0;}
        private static bool itob_i32_bool(int x)  {return x != 0;}
        private static bool itob_i64_bool(long x) {return x != 0;}
        
        private static sbyte zext_i8_i8(sbyte x)   {return (sbyte) ((byte)(x));}
        private static short zext_i8_i16(sbyte x)  {return (short)((byte)(x));}
        private static int   zext_i8_i32(sbyte x)  {return (int)((byte)(x));}
        private static long  zext_i8_i64(sbyte x)  {return (long)((byte)(x));}
        
        private static sbyte zext_i16_i8(short x)  {return (sbyte) ((ushort)(x));}
        private static short zext_i16_i16(short x) {return (short)((ushort)(x));}
        private static int   zext_i16_i32(short x) {return (int)((ushort)(x));}
        private static long  zext_i16_i64(short x) {return (long)((ushort)(x));}
        
        private static sbyte zext_i32_i8(int x){return (sbyte) ((uint)(x));}
        private static short zext_i32_i16(int x){return (short)((uint)(x));}
        private static int   zext_i32_i32(int x){return (int)((uint)(x));}
        private static long  zext_i32_i64(int x){return (long)((uint)(x));}
        
        private static sbyte zext_i64_i8(long x){return (sbyte) ((ulong)(x));}
        private static short zext_i64_i16(long x){return (short)((ulong)(x));}
        private static int   zext_i64_i32(long x){return (int)((ulong)(x));}
        private static long  zext_i64_i64(long x){return (long)((ulong)(x));}
        
        private static sbyte ssignum(sbyte x){return (sbyte) Math.Sign(x);}
        private static short ssignum(short x){return (short) Math.Sign(x);}
        private static int ssignum(int x){return Math.Sign(x);}
        private static long ssignum(long x){return (long) Math.Sign(x);}
        
        private static sbyte usignum(sbyte x){return ((byte) x > 0) ? (sbyte) 1 : (sbyte) 0;}
        private static short usignum(short x){return ((ushort) x > 0) ? (short) 1 : (short) 0;}
        private static int usignum(int x){return ((uint) x > 0) ? (int) 1 : (int) 0;}
        private static long usignum(long x){return ((ulong) x > 0) ? (long) 1 : (long) 0;}
        
        private static float sitofp_i8_f32(sbyte x){return Convert.ToSingle(x);}
        private static float sitofp_i16_f32(short x){return Convert.ToSingle(x);}
        private static float sitofp_i32_f32(int x){return Convert.ToSingle(x);}
        private static float sitofp_i64_f32(long x){return Convert.ToSingle(x);}
        
        private static double sitofp_i8_f64(sbyte x){return Convert.ToDouble(x);}
        private static double sitofp_i16_f64(short x){return Convert.ToDouble(x);}
        private static double sitofp_i32_f64(int x){return Convert.ToDouble(x);}
        private static double sitofp_i64_f64(long x){return Convert.ToDouble(x);}
        
        
        private static float uitofp_i8_f32(sbyte x){return Convert.ToSingle(unsigned(x));}
        private static float uitofp_i16_f32(short x){return Convert.ToSingle(unsigned(x));}
        private static float uitofp_i32_f32(int x){return Convert.ToSingle(unsigned(x));}
        private static float uitofp_i64_f32(long x){return Convert.ToSingle(unsigned(x));}
        
        private static double uitofp_i8_f64(sbyte x){return Convert.ToDouble(unsigned(x));}
        private static double uitofp_i16_f64(short x){return Convert.ToDouble(unsigned(x));}
        private static double uitofp_i32_f64(int x){return Convert.ToDouble(unsigned(x));}
        private static double uitofp_i64_f64(long x){return Convert.ToDouble(unsigned(x));}
        
        private static byte fptoui_f32_i8(float x){return (byte) (Math.Truncate(x));}
        private static byte fptoui_f64_i8(double x){return (byte) (Math.Truncate(x));}
        private static sbyte fptosi_f32_i8(float x){return (sbyte) (Math.Truncate(x));}
        private static sbyte fptosi_f64_i8(double x){return (sbyte) (Math.Truncate(x));}
        
        private static ushort fptoui_f32_i16(float x){return (ushort) (Math.Truncate(x));}
        private static ushort fptoui_f64_i16(double x){return (ushort) (Math.Truncate(x));}
        private static short fptosi_f32_i16(float x){return (short) (Math.Truncate(x));}
        private static short fptosi_f64_i16(double x){return (short) (Math.Truncate(x));}
        
        private static uint fptoui_f32_i32(float x){return (uint) (Math.Truncate(x));}
        private static uint fptoui_f64_i32(double x){return (uint) (Math.Truncate(x));}
        private static int fptosi_f32_i32(float x){return (int) (Math.Truncate(x));}
        private static int fptosi_f64_i32(double x){return (int) (Math.Truncate(x));}
        
        private static ulong fptoui_f32_i64(float x){return (ulong) (Math.Truncate(x));}
        private static ulong fptoui_f64_i64(double x){return (ulong) (Math.Truncate(x));}
        private static long fptosi_f32_i64(float x){return (long) (Math.Truncate(x));}
        private static long fptosi_f64_i64(double x){return (long) (Math.Truncate(x));}
        
        private static double fpconv_f32_f64(float x){return Convert.ToDouble(x);}
        private static float fpconv_f64_f32(double x){return Convert.ToSingle(x);}
        
        private static double futhark_log64(double x){return Math.Log(x);}
        private static double futhark_log2_64(double x){return Math.Log(x,2.0);}
        private static double futhark_log10_64(double x){return Math.Log10(x);}
        private static double futhark_sqrt64(double x){return Math.Sqrt(x);}
        private static double futhark_exp64(double x){return Math.Exp(x);}
        private static double futhark_cos64(double x){return Math.Cos(x);}
        private static double futhark_sin64(double x){return Math.Sin(x);}
        private static double futhark_tan64(double x){return Math.Tan(x);}
        private static double futhark_acos64(double x){return Math.Acos(x);}
        private static double futhark_asin64(double x){return Math.Asin(x);}
        private static double futhark_atan64(double x){return Math.Atan(x);}
        private static double futhark_atan2_64(double x, double y){return Math.Atan2(x, y);}
        private static double futhark_gamma64(double x){throw new NotImplementedException();}
        private static double futhark_lgamma64(double x){throw new NotImplementedException();}
        private static bool futhark_isnan64(double x){return double.IsNaN(x);}
        private static bool futhark_isinf64(double x){return double.IsInfinity(x);}
        private static long futhark_to_bits64(double x){return BitConverter.ToInt64(BitConverter.GetBytes(x),0);}
        private static double futhark_from_bits64(long x){return BitConverter.ToDouble(BitConverter.GetBytes(x),0);}
        
        private static float futhark_log32(float x){return (float) Math.Log(x);}
        private static float futhark_log2_32(float x){return (float) Math.Log(x,2.0);}
        private static float futhark_log10_32(float x){return (float) Math.Log10(x);}
        private static float futhark_sqrt32(float x){return (float) Math.Sqrt(x);}
        private static float futhark_exp32(float x){return (float) Math.Exp(x);}
        private static float futhark_cos32(float x){return (float) Math.Cos(x);}
        private static float futhark_sin32(float x){return (float) Math.Sin(x);}
        private static float futhark_tan32(float x){return (float) Math.Tan(x);}
        private static float futhark_acos32(float x){return (float) Math.Acos(x);}
        private static float futhark_asin32(float x){return (float) Math.Asin(x);}
        private static float futhark_atan32(float x){return (float) Math.Atan(x);}
        private static float futhark_atan2_32(float x, float y){return (float) Math.Atan2(x, y);}
        private static float futhark_gamma32(float x){throw new NotImplementedException();}
        private static float futhark_lgamma32(float x){throw new NotImplementedException();}
        private static bool futhark_isnan32(float x){return float.IsNaN(x);}
        private static bool futhark_isinf32(float x){return float.IsInfinity(x);}
        private static int futhark_to_bits32(float x){return BitConverter.ToInt32(BitConverter.GetBytes(x), 0);}
        private static float futhark_from_bits32(int x){return BitConverter.ToSingle(BitConverter.GetBytes(x), 0);}
        
        private static float futhark_round32(float x){return (float) Math.Round(x);}
        private static double futhark_round64(double x){return Math.Round(x);}
        private static float futhark_ceil32(float x){return (float) Math.Ceiling(x);}
        private static double futhark_ceil64(double x){return Math.Ceiling(x);}
        private static float futhark_floor32(float x){return (float) Math.Floor(x);}
        private static double futhark_floor64(double x){return Math.Floor(x);}
        
        private static float futhark_lerp32(float v0, float v1, float t){return v0 + (v1-v0)*t;}
        private static double futhark_lerp64(double v0, double v1, double t){return v0 + (v1-v0)*t;}
        
        int futhark_popc8 (sbyte x) {
          int c = 0;
          for (; x != 0; ++c) {
              x &= (sbyte)(x - 1);
          }
          return c;
         }
        
        int futhark_popc16 (short x) {
          int c = 0;
          for (; x != 0; ++c) {
              x &= (short)(x - 1);
          }
          return c;
        }
        
        int futhark_popc32 (int x) {
          int c = 0;
          for (; x != 0; ++c) {
              x &= x - 1;
          }
          return c;
        }
        
        int futhark_popc64 (long x) {
          int c = 0;
          for (; x != 0; ++c) {
              x &= x - 1;
          }
          return c;
        }
        
        int futhark_clzz8 (sbyte x) {
            int n = 0;
            int bits = sizeof(sbyte) * 8;
            for (int i = 0; i < bits; i++) {
                if (x < 0) break;
                n++;
                x <<= 1;
            }
            return n;
        }
        
        int futhark_clzz16 (short x) {
            int n = 0;
            int bits = sizeof(short) * 8;
            for (int i = 0; i < bits; i++) {
                if (x < 0) break;
                n++;
                x <<= 1;
            }
            return n;
        }
        
        int futhark_clzz32 (int x) {
            int n = 0;
            int bits = sizeof(int) * 8;
            for (int i = 0; i < bits; i++) {
                if (x < 0) break;
                n++;
                x <<= 1;
            }
            return n;
        }
        
        int futhark_clzz64 (long x) {
            int n = 0;
            int bits = sizeof(long) * 8;
            for (int i = 0; i < bits; i++) {
                if (x < 0) break;
                n++;
                x <<= 1;
            }
            return n;
        }
        
        private static bool llt (bool x, bool y){return (!x && y);}
        private static bool lle (bool x, bool y){return (!x || y);}
        
        public struct FlatArray<T>
        {
            public long[] shape;
            public T[] array;
        
            public FlatArray(T[] data_array, long[] shape_array)
            {
                shape = shape_array;
                array = data_array;
            }
        
            public FlatArray(T[] data_array)
            {
                shape = new long[] {data_array.Length};
                array = data_array;
            }
        
            private long getIdx(int[] idxs)
            {
                long idx = 0;
                for (int i = 0; i<idxs.Length; i++)
                {
                    idx += shape[i] * idxs[i];
                }
                return idx;
        
            }
            public T this[params int[] indexes]
            {
                get
                {
                    Debug.Assert(indexes.Length == shape.Length);
                    return array[getIdx(indexes)];
                }
        
                set
                {
                    Debug.Assert(indexes.Length == shape.Length);
                    array[getIdx(indexes)] = value;
                }
            }
        
            public IEnumerator GetEnumerator()
            {
                foreach (T val in array)
                {
                    yield return val;
                }
            }
        
            public (T[], long[]) AsTuple()
            {
                return (this.array, this.shape);
            }
        }
        
        public class Opaque{
            object desc;
            object data;
            public Opaque(string str, object payload)
            {
                this.desc = str;
                this.data = payload;
            }
        
            public override string ToString()
            {
                return string.Format("<opaque Futhark value of type {}>", desc);
            }
        }
        
        private byte[] allocateMem(sbyte size)
        {
            return new byte[size];
        }
        
        private byte[] allocateMem(short size)
        {
            return new byte[size];
        }
        
        private byte[] allocateMem(int size)
        {
            return new byte[size];
        }
        
        private byte[] allocateMem(long size)
        {
            return new byte[size];
        }
        
        private byte[] allocateMem(byte size)
        {
            return new byte[size];
        }
        
        private byte[] allocateMem(ushort size)
        {
            return new byte[size];
        }
        
        private byte[] allocateMem(uint size)
        {
            return new byte[size];
        }
        
        private byte[] allocateMem(ulong size)
        {
            return new byte[size];
        }
        
        private Tuple<byte[], long[]> createTuple_byte(byte[] bytes, long[] shape)
        {
            var byteArray = new byte[bytes.Length / sizeof(byte)];
            Buffer.BlockCopy(bytes, 0, byteArray, 0, bytes.Length);
            return Tuple.Create(byteArray, shape);
        }
        
        private Tuple<ushort[], long[]> createTuple_ushort(byte[] bytes, long[] shape)
        {
            var ushortArray = new ushort[bytes.Length / sizeof(ushort)];
            Buffer.BlockCopy(bytes, 0, ushortArray, 0, bytes.Length);
            return Tuple.Create(ushortArray, shape);
        }
        
        private Tuple<uint[], long[]> createTuple_uint(byte[] bytes, long[] shape)
        {
            var uintArray = new uint[bytes.Length / sizeof(uint)];
            Buffer.BlockCopy(bytes, 0, uintArray, 0, bytes.Length);
            return Tuple.Create(uintArray, shape);
        }
        
        private Tuple<ulong[], long[]> createTuple_ulong(byte[] bytes, long[] shape)
        {
            var ulongArray = new ulong[bytes.Length / sizeof(ulong)];
            Buffer.BlockCopy(bytes, 0, ulongArray, 0, bytes.Length);
            return Tuple.Create(ulongArray, shape);
        }
        
        
        private Tuple<sbyte[], long[]> createTuple_sbyte(byte[] bytes, long[] shape)
        {
            var sbyteArray = new sbyte[1];
            if (bytes.Length > 0)
            {
                sbyteArray = new sbyte[bytes.Length / sizeof(sbyte)];
            }
            Buffer.BlockCopy(bytes, 0, sbyteArray, 0, bytes.Length);
            return Tuple.Create(sbyteArray, shape);
        }
        
        
        private Tuple<short[], long[]> createTuple_short(byte[] bytes, long[] shape)
        {
            var shortArray = new short[1];
            if (bytes.Length > 0)
            {
                shortArray = new short[bytes.Length / sizeof(short)];
            }
            Buffer.BlockCopy(bytes, 0, shortArray, 0, bytes.Length);
            return Tuple.Create(shortArray, shape);
        }
        
        private Tuple<int[], long[]> createTuple_int(byte[] bytes, long[] shape)
        {
            var intArray = new int[1];
            if (bytes.Length > 0)
            {
                intArray = new int[bytes.Length / sizeof(int)];
            }
            Buffer.BlockCopy(bytes, 0, intArray, 0, bytes.Length);
            return Tuple.Create(intArray, shape);
        }
        
        private Tuple<long[], long[]> createTuple_long(byte[] bytes, long[] shape)
        {
            var longArray = new long[1];
            if (bytes.Length > 0)
            {
                longArray = new long[bytes.Length / sizeof(long)];
            }
            Buffer.BlockCopy(bytes, 0, longArray, 0, bytes.Length);
            return Tuple.Create(longArray, shape);
        }
        
        private Tuple<float[], long[]> createTuple_float(byte[] bytes, long[] shape)
        {
            var floatArray = new float[1];
            if (bytes.Length > 0)
            {
                floatArray = new float[bytes.Length / sizeof(float)];
            }
            Buffer.BlockCopy(bytes, 0, floatArray, 0, bytes.Length);
            return Tuple.Create(floatArray, shape);
        }
        
        
        private Tuple<double[], long[]> createTuple_double(byte[] bytes, long[] shape)
        {
            var doubleArray = new double[1];
            if (bytes.Length > 0)
            {
                doubleArray = new double[bytes.Length / sizeof(double)];
            }
            Buffer.BlockCopy(bytes, 0, doubleArray, 0, bytes.Length);
            return Tuple.Create(doubleArray, shape);
        }
        
        private Tuple<bool[], long[]> createTuple_bool(byte[] bytes, long[] shape)
        {
            var boolArray = new bool[1];
            if (bytes.Length > 0)
            {
                boolArray = new bool[bytes.Length / sizeof(bool)];
            }
            Buffer.BlockCopy(bytes, 0, boolArray, 0, bytes.Length);
            return Tuple.Create(boolArray, shape);
        }
        
        private byte[] unwrapArray(Array src, int obj_size)
        {
            var bytes = new byte[src.Length * obj_size];
            Buffer.BlockCopy(src, 0, bytes, 0, bytes.Length);
            return bytes;
        }
        
        private byte indexArray_byte(byte[] src, int offset)
        {
            unsafe
            {
                fixed (void* dest_ptr = &src[offset])
                {
                    return *(byte*) dest_ptr;
                }
            }
        }
        
        private ushort indexArray_ushort(byte[] src, int offset)
        {
            unsafe
            {
                fixed (void* dest_ptr = &src[offset])
                {
                    return *(ushort*) dest_ptr;
                }
            }
        }
        
        private uint indexArray_uint(byte[] src, int offset)
        {
            unsafe
            {
                fixed (void* dest_ptr = &src[offset])
                {
                    return *(uint*) dest_ptr;
                }
            }
        }
        
        private ulong indexArray_ulong(byte[] src, int offset)
        {
            unsafe
            {
                fixed (void* dest_ptr = &src[offset])
                {
                    return *(ulong*) dest_ptr;
                }
            }
        }
        
        private sbyte indexArray_sbyte(byte[] src, int offset)
        {
            unsafe
            {
                fixed (void* dest_ptr = &src[offset])
                {
                    return *(sbyte*) dest_ptr;
                }
            }
        }
        
        private short indexArray_short(byte[] src, int offset)
        {
            unsafe
            {
                fixed (void* dest_ptr = &src[offset])
                {
                    return *(short*) dest_ptr;
                }
            }
        }
        
        private int indexArray_int(byte[] src, int offset)
        {
            unsafe
            {
                fixed (void* dest_ptr = &src[offset])
                {
                    return *(int*) dest_ptr;
                }
            }
        }
        
        private long indexArray_long(byte[] src, int offset)
        {
            unsafe
            {
                fixed (void* dest_ptr = &src[offset])
                {
                    return *(long*) dest_ptr;
                }
            }
        }
        
        private float indexArray_float(byte[] src, int offset)
        {
            unsafe
            {
                fixed (void* dest_ptr = &src[offset])
                {
                    return *(float*) dest_ptr;
                }
            }
        }
        
        private double indexArray_double(byte[] src, int offset)
        {
            unsafe
            {
                fixed (void* dest_ptr = &src[offset])
                {
                    return *(double*) dest_ptr;
                }
            }
        }
        
        private bool indexArray_bool(byte[] src, int offset)
        {
            unsafe
            {
                fixed (void* dest_ptr = &src[offset])
                {
                    return *(bool*) dest_ptr;
                }
            }
        }
        
        private void writeScalarArray(byte[] dest, int offset, sbyte value)
        {
            unsafe
            {
                fixed (byte* dest_ptr = &dest[offset])
                {
                    *(sbyte*) dest_ptr = value;
                }
            }
        }
        private void writeScalarArray(byte[] dest, int offset, byte value)
        {
            unsafe
            {
                fixed (byte* dest_ptr = &dest[offset])
                {
                    *(byte*) dest_ptr = value;
                }
            }
        }
        private void writeScalarArray(byte[] dest, int offset, short value)
        {
            unsafe
            {
                fixed (byte* dest_ptr = &dest[offset])
                {
                    *(short*) dest_ptr = value;
                }
            }
        }
        private void writeScalarArray(byte[] dest, int offset, ushort value)
        {
            unsafe
            {
                fixed (byte* dest_ptr = &dest[offset])
                {
                    *(ushort*) dest_ptr = value;
                }
            }
        }
        private void writeScalarArray(byte[] dest, int offset, int value)
        {
            unsafe
            {
                fixed (byte* dest_ptr = &dest[offset])
                {
                    *(int*) dest_ptr = value;
                }
            }
        }
        private void writeScalarArray(byte[] dest, int offset, uint value)
        {
            unsafe
            {
                fixed (byte* dest_ptr = &dest[offset])
                {
                    *(uint*) dest_ptr = value;
                }
            }
        }
        private void writeScalarArray(byte[] dest, int offset, long value)
        {
            unsafe
            {
                fixed (byte* dest_ptr = &dest[offset])
                {
                    *(long*) dest_ptr = value;
                }
            }
        }
        private void writeScalarArray(byte[] dest, int offset, ulong value)
        {
            unsafe
            {
                fixed (byte* dest_ptr = &dest[offset])
                {
                    *(ulong*) dest_ptr = value;
                }
            }
        }
        private void writeScalarArray(byte[] dest, int offset, float value)
        {
            unsafe
            {
                fixed (byte* dest_ptr = &dest[offset])
                {
                    *(float*) dest_ptr = value;
                }
            }
        }
        private void writeScalarArray(byte[] dest, int offset, double value)
        {
            unsafe
            {
                fixed (byte* dest_ptr = &dest[offset])
                {
                    *(double*) dest_ptr = value;
                }
            }
        }
        private void writeScalarArray(byte[] dest, int offset, bool value)
        {
            unsafe
            {
                fixed (byte* dest_ptr = &dest[offset])
                {
                    *(bool*) dest_ptr = value;
                }
            }
        }
        private void panic(int exitcode, string str, params Object[] args)
        {
            var prog_name = Environment.GetCommandLineArgs()[0];
            Console.Error.WriteLine(String.Format("{0}:", prog_name));
            Console.Error.WriteLine(String.Format(str, args));
            Environment.Exit(exitcode);
        }
        
        private void FutharkAssert(bool assertion)
        {
            if (!assertion)
            {
                Environment.Exit(1);
            }
        }
        
        private void FutharkAssert(bool assertion, string errorMsg)
        {
            if (!assertion)
            {
                Console.Error.WriteLine(errorMsg);
                Environment.Exit(1);
            }
        }
        private class ValueError : Exception
        {
            public ValueError(){}
            public ValueError(string message):base(message){}
            public ValueError(string message, Exception inner):base(message, inner){}
        }
        private Stream s;
        private BinaryReader b;
        
        // Note that the lookahead buffer does not interact well with
        // binary reading.  We are careful to not let this become a
        // problem.
        private Stack<char> LookaheadBuffer = new Stack<char>();
        
        private void ResetLookahead(){
            LookaheadBuffer.Clear();
        }
        
        private void ValueReader(Stream s)
        {
            this.s = s;
        }
        
        private void ValueReader()
        {
            this.s = Console.OpenStandardInput();
            this.b = new BinaryReader(s);
        }
        
        private char? GetChar()
        {
            char c;
            if (LookaheadBuffer.Count == 0)
            {
                c = (char) this.b.ReadByte();
            }
            else
            {
                c = LookaheadBuffer.Pop();
            }
        
            return c;
        }
        
        private char[] GetChars(int n)
        {
            return Enumerable.Range(0, n).Select(_ => GetChar().Value).ToArray();
        }
        
        private void UngetChar(char c)
        {
            LookaheadBuffer.Push(c);
        }
        
        private char PeekChar()
        {
            var c = GetChar();
            UngetChar(c.Value);
            return c.Value;
        }
        
        private void SkipSpaces()
        {
            var c = GetChar();
            while (c.HasValue){
                if (char.IsWhiteSpace(c.Value))
                {
                    c = GetChar();
                }
                else if (c == '-')
                {
                    if (PeekChar() == '-')
                    {
                        while (c.Value != '\n')
                        {
                            c = GetChar();
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        
            if (c.HasValue)
            {
                UngetChar(c.Value);
            }
        }
        
        private bool ParseSpecificChar(char c)
        {
            var got = GetChar();
            if (got.Value != c)
            {
                UngetChar(got.Value);
                throw new ValueError();
            }
            return true;
        }
        
        private bool ParseSpecificString(string str)
        {
            var read = new List<char>();
            foreach (var c in str.ToCharArray())
            {
                try
                {
                    ParseSpecificChar(c);
                    read.Add(c);
                }
                catch(ValueError)
                {
                    read.Reverse();
                    foreach (var cc in read)
                    {
                        UngetChar(cc);
                    }
                    throw;
                }
            }
        
            return true;
        }
        
        private string Optional(Func<string> p)
        {
            string res = null;
            try
            {
                res = p();
            }
            catch (Exception)
            {
            }
        
            return res;
        }
        
        private bool Optional(Func<char, bool> p, char c)
        {
            try
            {
                return p(c);
            }
            catch (Exception)
            {
            }
        
            return false;
        }
        
        private bool OptionalSpecificString(string s)
        {
            var c = PeekChar();
            if (c == s[0])
            {
                return ParseSpecificString(s);
            }
            return false;
        }
        
        
        private List<string> sepBy(Func<string> p, Func<string> sep)
        {
            var elems = new List<string>();
            var x = Optional(p);
            if (!string.IsNullOrWhiteSpace(x))
            {
                elems.Add(x);
                while (!string.IsNullOrWhiteSpace(Optional(sep)))
                {
                    var y = Optional(p);
                    elems.Add(y);
                }
            }
            return elems;
        }
        
        private string ParseHexInt()
        {
            var s = "";
            var c = GetChar();
            while (c.HasValue)
            {
                if (Uri.IsHexDigit(c.Value))
                {
                    s += c.Value;
                    c = GetChar();
                }
                else if (c == '_')
                {
                    c = GetChar();
                }
                else
                {
                    UngetChar(c.Value);
                    break;
                }
            }
        
            return Convert.ToString(Convert.ToUInt32(s, 16));
        }
        
        private string ParseInt()
        {
            var s = "";
            var c = GetChar();
            if (c.Value == '0' && "xX".Contains(PeekChar()))
            {
                GetChar();
                s += ParseHexInt();
            }
            else
            {
                while (c.HasValue)
                {
                    if (char.IsDigit(c.Value))
                    {
                        s += c.Value;
                        c = GetChar();
                    }else if (c == '_')
                    {
                        c = GetChar();
                    }
                    else
                    {
                        UngetChar(c.Value);
                        break;
                    }
                }
        
            }
        
            if (s.Length == 0)
            {
                throw new Exception("ValueError");
            }
        
            return s;
        }
        
        private string ParseIntSigned()
        {
            var c = GetChar();
            if (c.Value == '-' && char.IsDigit(PeekChar()))
            {
                return c + ParseInt();
            }
            else
            {
                if (c.Value != '+')
                {
                    UngetChar(c.Value);
                }
        
                return ParseInt();
            }
        }
        
        private string ReadStrComma()
        {
            SkipSpaces();
            ParseSpecificChar(',');
            return ",";
        }
        
        private int ReadStrInt(string s)
        {
            SkipSpaces();
            var x = Convert.ToInt32(ParseIntSigned());
            OptionalSpecificString(s);
            return x;
        }
        
        private ulong ReadStrUInt64(string s)
        {
            SkipSpaces();
            var x = Convert.ToUInt64(ParseInt());
            OptionalSpecificString(s);
            return x;
        }
        
        private long ReadStrInt64(string s)
        {
            SkipSpaces();
            var x = Convert.ToInt64(ParseIntSigned());
            OptionalSpecificString(s);
            return x;
        }
        
        private uint ReadStrUInt(string s)
        {
            SkipSpaces();
            var x = Convert.ToUInt32(ParseInt());
            OptionalSpecificString(s);
            return x;
        }
        
        private int ReadStrI8(){return ReadStrInt("i8");}
        private int ReadStrI16(){return ReadStrInt("i16");}
        private int ReadStrI32(){return ReadStrInt("i32");}
        private long ReadStrI64(){return ReadStrInt64("i64");}
        private uint ReadStrU8(){return ReadStrUInt("u8");}
        private uint ReadStrU16(){return ReadStrUInt("u16");}
        private uint ReadStrU32(){return ReadStrUInt("u32");}
        private ulong ReadStrU64(){return ReadStrUInt64("u64");}
        private sbyte ReadBinI8(){return (sbyte) b.ReadByte();}
        private short ReadBinI16(){return b.ReadInt16();}
        private int ReadBinI32(){return b.ReadInt32();}
        private long ReadBinI64(){return b.ReadInt64();}
        private byte ReadBinU8(){return (byte) b.ReadByte();}
        private ushort ReadBinU16(){return b.ReadUInt16();}
        private uint ReadBinU32(){return b.ReadUInt32();}
        private ulong ReadBinU64(){return b.ReadUInt64();}
        private float ReadBinF32(){return b.ReadSingle();}
        private double ReadBinF64(){return b.ReadDouble();}
        private bool ReadBinBool(){return b.ReadBoolean();}
        
        private char ReadChar()
        {
            SkipSpaces();
            ParseSpecificChar('\'');
            var c = GetChar();
            ParseSpecificChar('\'');
            return c.Value;
        }
        
        private double ReadStrHexFloat(char sign)
        {
            var int_part = ParseHexInt();
            ParseSpecificChar('.');
            var frac_part = ParseHexInt();
            ParseSpecificChar('p');
            var exponent = ParseHexInt();
        
            var int_val = Convert.ToInt32(int_part, 16);
            var frac_val = Convert.ToSingle(Convert.ToInt32(frac_part, 16)) / Math.Pow(16, frac_part.Length);
            var exp_val = Convert.ToInt32(exponent);
        
            var total_val = (int_val + frac_val) * Math.Pow(2, exp_val);
            if (sign == '-')
            {
                total_val = -1 * total_val;
            }
        
            return Convert.ToDouble(total_val);
        }
        
        private double ReadStrDecimal()
        {
            SkipSpaces();
            var c = GetChar();
            char sign;
            if (c.Value == '-')
            {
                sign = '-';
            }
            else
            {
                UngetChar(c.Value);
                sign = '+';
            }
        
            // Check for hexadecimal float
            c = GetChar();
            if (c.Value == '0' && "xX".Contains(PeekChar()))
            {
                GetChar();
                return ReadStrHexFloat(sign);
            }
            else
            {
                UngetChar(c.Value);
            }
        
            var bef = Optional(this.ParseInt);
            var aft = "";
            if (string.IsNullOrEmpty(bef))
            {
                bef = "0";
                ParseSpecificChar('.');
                aft = ParseInt();
            }else if (Optional(ParseSpecificChar, '.'))
            {
                aft = ParseInt();
            }
            else
            {
                aft = "0";
            }
        
            var expt = "";
            if (Optional(ParseSpecificChar, 'E') ||
                Optional(ParseSpecificChar, 'e'))
            {
                expt = ParseIntSigned();
            }
            else
            {
                expt = "0";
            }
        
            return Convert.ToDouble(sign + bef + "." + aft + "E" + expt);
        }
        
        private float ReadStrF32()
        {
            try
            {
                ParseSpecificString("f32.nan");
                return Single.NaN;
            }
            catch (ValueError)
            {
                try
                {
                    ParseSpecificString("-f32.inf");
                    return Single.NegativeInfinity;
                }
                catch (ValueError)
                {
                    try
                    {
                        ParseSpecificString("f32.inf");
                        return Single.PositiveInfinity;
                    }
                    catch (ValueError)
                    {
                        var x = ReadStrDecimal();
                        OptionalSpecificString("f32");
                        return Convert.ToSingle(x);
                    }
                }
            }
        }
        
        private double ReadStrF64()
        {
            try
            {
                ParseSpecificString("f64.nan");
                return Double.NaN;
            }
            catch (ValueError)
            {
                try
                {
                    ParseSpecificString("-f64.inf");
                    return Double.NegativeInfinity;
                }
                catch (ValueError)
                {
                    try
                    {
                        ParseSpecificString("f64.inf");
                        return Double.PositiveInfinity;
                    }
                    catch (ValueError)
                    {
                        var x = ReadStrDecimal();
                        OptionalSpecificString("f64");
                        return x;
                    }
                }
            }
        }
        private bool ReadStrBool()
        {
            SkipSpaces();
            if (PeekChar() == 't')
            {
                ParseSpecificString("true");
                return true;
            }
        
            if (PeekChar() == 'f')
            {
                ParseSpecificString("false");
                return false;
            }
        
            throw new ValueError();
        }
        
        private (T[], int[]) ReadStrArrayElems<T>(int rank, Func<T> ReadStrScalar)
        {
            bool first = true;
            bool[] knows_dimsize = new bool[rank];
            int cur_dim = rank-1;
            int[] elems_read_in_dim = new int[rank];
            int[] shape = new int[rank];
        
            int capacity = 100;
            T[] data = new T[capacity];
            int write_ptr = 0;
        
            while (true) {
                SkipSpaces();
        
                char c = (char) GetChar();
                if (c == ']') {
                    if (knows_dimsize[cur_dim]) {
                        if (shape[cur_dim] != elems_read_in_dim[cur_dim]) {
                            throw new Exception("Irregular array");
                        }
                    } else {
                        knows_dimsize[cur_dim] = true;
                        shape[cur_dim] = elems_read_in_dim[cur_dim];
                    }
                    if (cur_dim == 0) {
                        break;
                    } else {
                        cur_dim--;
                        elems_read_in_dim[cur_dim]++;
                    }
                } else if (c == ',') {
                    SkipSpaces();
                    c = (char) GetChar();
                    if (c == '[') {
                        if (cur_dim == rank - 1) {
                            throw new Exception("Array has too many dimensions");
                        }
                        first = true;
                        cur_dim++;
                        elems_read_in_dim[cur_dim] = 0;
                    } else if (cur_dim == rank - 1) {
                        UngetChar(c);
        
                        data[write_ptr++] = ReadStrScalar();
                        if (write_ptr == capacity) {
                            capacity *= 2;
                            Array.Resize(ref data, capacity);
                        }
                        elems_read_in_dim[cur_dim]++;
                    } else {
                        throw new Exception("Unexpected comma when reading array");
                    }
                } else if (first) {
                    if (c == '[') {
                        if (cur_dim == rank - 1) {
                            throw new Exception("Array has too many dimensions");
                        }
                        cur_dim++;
                        elems_read_in_dim[cur_dim] = 0;
                    } else {
                        UngetChar(c);
                        data[write_ptr++] = ReadStrScalar();
                        if (write_ptr == capacity) {
                            capacity *= 2;
                            Array.Resize(ref data, capacity);
                        }
                        elems_read_in_dim[cur_dim]++;
                        first = false;
                    }
                } else {
                    throw new Exception("Unexpected character in array");
                }
            }
            Array.Resize(ref data, write_ptr);
            return (data, shape);
        }
        
        private (T[], int[]) ReadStrArrayEmpty<T>(int rank, string typeName, Func<T> ReadStrScalar)
        {
            ParseSpecificString("empty");
            ParseSpecificChar('(');
            int[] shape = new int[rank];
            for (int i = 0; i < rank; i++) {
                ParseSpecificString("[");
                shape[i] = Convert.ToInt32(ParseIntSigned());
                ParseSpecificString("]");
            }
            ParseSpecificString(typeName);
            ParseSpecificChar(')');
        
            // Check whether the array really is empty.
            for (int i = 0; i < rank; i++) {
                if (shape[i] == 0) {
                    return (new T[1], shape);
                }
            }
        
            // Not an empty array!
            throw new Exception("empty() used with nonempty shape");
        }
        
        private (T[], int[]) ReadStrArray<T>(int rank, string typeName, Func<T> ReadStrScalar)
        {
            long read_dims = 0;
        
            while (true) {
                SkipSpaces();
                var c = GetChar();
                if (c=='[') {
                    read_dims++;
                } else {
                    if (c != null) {
                        UngetChar((char)c);
                    }
                    break;
                }
            }
        
            if (read_dims == 0) {
                return ReadStrArrayEmpty(rank, typeName, ReadStrScalar);
            }
        
            if (read_dims != rank) {
                throw new Exception("Wrong number of dimensions");
            }
        
            return ReadStrArrayElems(rank, ReadStrScalar);
        }
        
        private Dictionary<string, string> primtypes = new Dictionary<string, string>
        {
            {"  i8",   "i8"},
            {" i16",  "i16"},
            {" i32",  "i32"},
            {" i64",  "i64"},
            {"  u8",   "u8"},
            {" u16",  "u16"},
            {" u32",  "u32"},
            {" u64",  "u64"},
            {" f32",  "f32"},
            {" f64",  "f64"},
            {"bool", "bool"}
        };
        
        private int BINARY_FORMAT_VERSION = 2;
        
        
        private void read_le_2byte(ref short dest)
        {
            dest = b.ReadInt16();
        }
        
        private void read_le_4byte(ref int dest)
        {
            dest = b.ReadInt32();
        }
        
        private void read_le_8byte(ref long dest)
        {
            dest = b.ReadInt64();
        }
        
        private bool ReadIsBinary()
            {
                SkipSpaces();
                var c = GetChar();
                if (c == 'b')
                {
                    byte bin_version = new byte();
                    try
                    {
                        bin_version = (byte) b.ReadByte();
                    }
                    catch
                    {
                        Console.WriteLine("binary-input: could not read version");
                        Environment.Exit(1);
                    }
        
                    if (bin_version != BINARY_FORMAT_VERSION)
                    {
                        Console.WriteLine((
                            "binary-input: File uses version {0}, but I only understand version {1}.", bin_version,
                            BINARY_FORMAT_VERSION));
                        Environment.Exit(1);
                    }
        
                    return true;
                }
                UngetChar((char) c);
                return false;
            }
        
        private (T[], int[]) ReadArray<T>(int rank, string typeName, Func<T> ReadStrScalar)
        {
            if (!ReadIsBinary())
            {
                return ReadStrArray<T>(rank, typeName, ReadStrScalar);
            }
            else
            {
                return ReadBinArray<T>(rank, typeName, ReadStrScalar);
            }
        }
        private T ReadScalar<T>(string typeName, Func<T> ReadStrScalar, Func<T> ReadBinScalar)
        {
            if (!ReadIsBinary())
            {
                return ReadStrScalar();
            }
            else
            {
                ReadBinEnsureScalar(typeName);
                return ReadBinScalar();
            }
        }
        
        private void ReadBinEnsureScalar(string typeName)
        {
            var bin_dims = b.ReadByte();
            if (bin_dims != 0)
            {
                Console.WriteLine("binary-input: Expected scalar (0 dimensions), but got array with {0} dimensions.", bin_dims);
                Environment.Exit(1);
            }
        
            var bin_type = ReadBinReadTypeString();
            if (bin_type != typeName)
            {
                Console.WriteLine("binary-input: Expected scalar of type {0} but got scalar of type {1}.", typeName,
                                  bin_type);
                Environment.Exit(1);
            }
        }
        
        private string ReadBinReadTypeString()
        {
            var str_bytes = b.ReadBytes(4);
            var str = System.Text.Encoding.UTF8.GetString(str_bytes, 0, 4);
            return primtypes[str];
        }
        
        private (T[], int[]) ReadBinArray<T>(int rank, string typeName, Func<T> ReadStrScalar)
        {
            var bin_dims = new int();
            var shape = new int[rank];
            try
            {
                bin_dims = b.ReadByte();
            }
            catch
            {
                Console.WriteLine("binary-input: Couldn't get dims.");
                Environment.Exit(1);
            }
        
            if (bin_dims != rank)
            {
                Console.WriteLine("binary-input: Expected {0} dimensions, but got array with {1} dimensions", rank,
                    bin_dims);
                Environment.Exit(1);
        
            }
        
            var bin_primtype = ReadBinReadTypeString();
            if (typeName != bin_primtype)
            {
                Console.WriteLine("binary-input: Expected {0}D-array with element type '{1}', but got {2}D-array with element type '{3}'.",
                                  rank, typeName, bin_dims, bin_primtype);
                Environment.Exit(1);
            }
        
            int elem_count = 1;
            for (var i = 0; i < rank; i++)
            {
                long bin_shape = new long();
                try
                {
                    read_le_8byte(ref bin_shape);
                }
                catch
                {
                    Console.WriteLine("binary-input: Couldn't read size for dimension {0} of array.", i);
                    Environment.Exit(1);
                }
        
                elem_count *= (int) bin_shape;
                shape[i] = (int) bin_shape;
            }
        
            // For whatever reason, Marshal.SizeOf<bool> is 4, so special-case that here.
            var elem_size = typeof(T) == typeof(bool) ? 1 : Marshal.SizeOf<T>();
            var num_bytes = elem_count * elem_size;
            var tmp = new byte[num_bytes];
            var data = new T[elem_count];
        
            var to_read = num_bytes;
            var have_read = 0;
            while (to_read > 0)
            {
                var bytes_read = b.Read(tmp, have_read, to_read);
                to_read -= bytes_read;
                have_read += bytes_read;
                if (bytes_read == 0) {
                    Console.WriteLine("binary-input: EOF after {0} bytes (expected {1})", have_read, num_bytes);
                    Environment.Exit(1);
                }
            }
        
            if (!BitConverter.IsLittleEndian && elem_size != 1)
            {
                for (int i = 0; i < elem_count; i ++)
                {
                    Array.Reverse(tmp, i * elem_size, elem_size); 
                }
            }
            Buffer.BlockCopy(tmp,0,data,0,num_bytes);
        
            /* we should have a proper error message here */
            return (data, shape);
        }
        
        
        private sbyte ReadI8()
        {
            return (sbyte) ReadStrI8();
        }
        private short ReadI16()
        {
            return (short) ReadStrI16();
        }
        private int ReadI32()
        {
            return ReadStrI32();
        }
        private long ReadI64()
        {
            return ReadStrI64();
        }
        
        private byte ReadU8()
        {
            return (byte) ReadStrU8();
        }
        private ushort ReadU16()
        {
            return (ushort) ReadStrU16();
        }
        private uint ReadU32()
        {
            return (uint) ReadStrU32();
        }
        private ulong ReadU64()
        {
            return (ulong) ReadStrU64();
        }
        private bool ReadBool()
        {
            return ReadStrBool();
        }
        private float ReadF32()
        {
            return ReadStrF32();
        }
        private double ReadF64()
        {
            return ReadStrF64();
        }
        
        private void WriteValue(bool x){Console.Write(x ? "true" : "false", x);}
        private void WriteValue(sbyte x){Console.Write("{0}i8", x);}
        private void WriteValue(short x){Console.Write("{0}i16", x);}
        private void WriteValue(int x){Console.Write("{0}i32", x);}
        private void WriteValue(long x){Console.Write("{0}i64", x);}
        private void WriteValue(byte x){Console.Write("{0}u8", x);}
        private void WriteValue(ushort x){Console.Write("{0}u16", x);}
        private void WriteValue(uint x){Console.Write("{0}u32", x);}
        private void WriteValue(ulong x){Console.Write("{0}u64", x);}
        private void WriteValue(float x){if (Single.IsNaN(x))
            {Console.Write("f32.nan");} else if (Single.IsNegativeInfinity(x))
            {Console.Write("-f32.inf");} else if (Single.IsPositiveInfinity(x))
            {Console.Write("f32.inf");} else
            {Console.Write("{0:0.000000}f32", x);}}
        private void WriteValue(double x){if (Double.IsNaN(x))
            {Console.Write("f64.nan");} else if (Double.IsNegativeInfinity(x))
            {Console.Write("-f64.inf");} else if (Double.IsPositiveInfinity(x))
            {Console.Write("f64.inf");} else
            {Console.Write("{0:0.000000}f64", x);}}
        private int futhark_main(byte[] x_mem_4231, byte[] y_mem_4232,
                                 int implz2081U_4215, int implz2081U_4216)
        {
            var scalar_out_4233 = new int();
            bool dim_match_4219;
            dim_match_4219 = (implz2081U_4215 == implz2081U_4216);
            var empty_or_match_cert_4220 = true;
            FutharkAssert(dim_match_4219, String.Format("Error: {0}\n\nBacktrace:\n{1}"
                                                        ,
                                                        "function arguments of wrong shape"
                                                        ,
                                                        "-> #0  dotprod.fut:1:1-2:29\n"));
            int res_4222;
            int redout_4229;
            redout_4229 = (int) 0;
            var i_4230 = (int) 0;
            var one_4236 = (int) 1;
            for (int counter_4235 = 0 ;counter_4235 < implz2081U_4215 ;counter_4235++)
            {
                int x_4226;
                x_4226 = indexArray_int(x_mem_4231, i_4230);
                int x_4227;
                x_4227 = indexArray_int(y_mem_4232, i_4230);
                int res_4228;
                res_4228 = mul32(x_4226, x_4227);
                int res_4225;
                res_4225 = add32(res_4228, redout_4229);
                int redout_tmp_4234;
                redout_tmp_4234 = res_4225;
                redout_4229 = redout_tmp_4234;
                i_4230 += one_4236;
            }
            res_4222 = redout_4229;
            scalar_out_4233 = res_4222;
            return scalar_out_4233;
        }
        private void entry_main()
        {
            var x_mem_4231_ext = ReadArray<int>(1, "i32", ReadI32);
            var y_mem_4232_ext = ReadArray<int>(1, "i32", ReadI32);
            var implz2081U_4215 = (int) x_mem_4231_ext.Item2[0];
            var x_mem_4231 = unwrapArray(x_mem_4231_ext.Item1, sizeof(int));
            var implz2081U_4216 = (int) y_mem_4232_ext.Item2[0];
            var y_mem_4232 = unwrapArray(y_mem_4232_ext.Item1, sizeof(int));
            byte[] x_mem_copy_4237;
            byte[] y_mem_copy_4238;
            var x_mem_4231_nbytes = x_mem_4231.Length;
            var y_mem_4232_nbytes = y_mem_4232.Length;
            var scalar_out_4233 = new int();
            try
            {
                if (DoWarmupRun)
                {
                    x_mem_copy_4237 = allocateMem(x_mem_4231_nbytes);
                    Buffer.BlockCopy(x_mem_4231, 0, x_mem_copy_4237, 0,
                                     x_mem_4231_nbytes);
                    y_mem_copy_4238 = allocateMem(y_mem_4232_nbytes);
                    Buffer.BlockCopy(y_mem_4232, 0, y_mem_copy_4238, 0,
                                     y_mem_4232_nbytes);
                    scalar_out_4233 = futhark_main(x_mem_copy_4237,
                                                   y_mem_copy_4238,
                                                   implz2081U_4215,
                                                   implz2081U_4216);
                }
                for (int i = 0 ;i < NumRuns ;i++)
                {
                    x_mem_copy_4237 = allocateMem(x_mem_4231_nbytes);
                    Buffer.BlockCopy(x_mem_4231, 0, x_mem_copy_4237, 0,
                                     x_mem_4231_nbytes);
                    y_mem_copy_4238 = allocateMem(y_mem_4232_nbytes);
                    Buffer.BlockCopy(y_mem_4232, 0, y_mem_copy_4238, 0,
                                     y_mem_4232_nbytes);
                    var StopWatch = new Stopwatch();
                    StopWatch.Start();
                    scalar_out_4233 = futhark_main(x_mem_copy_4237,
                                                   y_mem_copy_4238,
                                                   implz2081U_4215,
                                                   implz2081U_4216);
                    StopWatch.Stop();
                    var timeElapsed = (StopWatch.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000));
                    if ((RuntimeFile != null))
                    {
                        RuntimeFileWriter.WriteLine(timeElapsed.ToString());
                    }
                    if ((i < (NumRuns - 1)))
                    {
                        
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(String.Format("Assertion.{0} failed",
                                                      e));
                Environment.Exit(1);
            }
            if ((RuntimeFile != null))
            {
                RuntimeFileWriter.Close();
                RuntimeFile.Close();
            }
            WriteValue((int) scalar_out_4233);
            Console.Write("\n");
        }
        public void InternalEntry()
        {
            var EntryPoints = new Dictionary<string, Action>{{"main",entry_main}};
            if (!EntryPoints.ContainsKey(EntryPoint))
            {
                Console.Error.WriteLine(string.Format("No entry point '{0}'.  Select another with --entry point.  Options are:\n{1}",
                                                      EntryPoint,
                                                      string.Join("\n",
                                                                  EntryPoints.Keys)));
                Environment.Exit(1);
            }
            else
            {
                var entryPointFun = EntryPoints[EntryPoint];
                entryPointFun.Invoke();
            }
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            var internalInstance = new FutharkInternal(args);
            internalInstance.InternalEntry();
        }
    }
}