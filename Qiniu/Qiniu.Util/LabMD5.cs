namespace Qiniu.Util
{
	public class LabMD5
	{
		private sealed class Digest
		{
			public uint A;

			public uint B;

			public uint C;

			public uint D;

			public Digest()
			{
				A = 1732584193u;
				B = 4023233417u;
				C = 2562383102u;
				D = 271733878u;
			}

			public override string ToString()
			{
				return BitHelper.ReverseByte(A).ToString("x8") + BitHelper.ReverseByte(B).ToString("x8") + BitHelper.ReverseByte(C).ToString("x8") + BitHelper.ReverseByte(D).ToString("x8");
			}
		}

		private static class BitHelper
		{
			public static uint RotateLeft(uint num, ushort shift)
			{
				return (num >> 32 - shift) | (num << (int)shift);
			}

			public static uint ReverseByte(uint num)
			{
				return ((num & 0xFF) << 24) | (num >> 24) | ((num & 0xFF0000) >> 8) | ((num & 0xFF00) << 8);
			}
		}

		private static readonly uint[] T = new uint[64]
		{
			3614090360u, 3905402710u, 606105819u, 3250441966u, 4118548399u, 1200080426u, 2821735955u, 4249261313u, 1770035416u, 2336552879u,
			4294925233u, 2304563134u, 1804603682u, 4254626195u, 2792965006u, 1236535329u, 4129170786u, 3225465664u, 643717713u, 3921069994u,
			3593408605u, 38016083u, 3634488961u, 3889429448u, 568446438u, 3275163606u, 4107603335u, 1163531501u, 2850285829u, 4243563512u,
			1735328473u, 2368359562u, 4294588738u, 2272392833u, 1839030562u, 4259657740u, 2763975236u, 1272893353u, 4139469664u, 3200236656u,
			681279174u, 3936430074u, 3572445317u, 76029189u, 3654602809u, 3873151461u, 530742520u, 3299628645u, 4096336452u, 1126891415u,
			2878612391u, 4237533241u, 1700485571u, 2399980690u, 4293915773u, 2240044497u, 1873313359u, 4264355552u, 2734768916u, 1309151649u,
			4149444226u, 3174756917u, 718787259u, 3951481745u
		};

		private uint[] X = new uint[16];

		public string ComputeHash(byte[] bytes)
		{
			Digest digest = new Digest();
			byte[] array = CreatePaddedBuffer(bytes);
			uint num = (uint)(array.Length * 8) / 32u;
			for (uint num2 = 0u; num2 < num / 16u; num2++)
			{
				CopyBlock(array, num2);
				Transform(ref digest.A, ref digest.B, ref digest.C, ref digest.D);
			}
			return digest.ToString();
		}

		private void Transform(ref uint A, ref uint B, ref uint C, ref uint D)
		{
			uint num = A;
			uint num2 = B;
			uint num3 = C;
			uint num4 = D;
			F(ref A, B, C, D, 0u, 7, 1u);
			F(ref D, A, B, C, 1u, 12, 2u);
			F(ref C, D, A, B, 2u, 17, 3u);
			F(ref B, C, D, A, 3u, 22, 4u);
			F(ref A, B, C, D, 4u, 7, 5u);
			F(ref D, A, B, C, 5u, 12, 6u);
			F(ref C, D, A, B, 6u, 17, 7u);
			F(ref B, C, D, A, 7u, 22, 8u);
			F(ref A, B, C, D, 8u, 7, 9u);
			F(ref D, A, B, C, 9u, 12, 10u);
			F(ref C, D, A, B, 10u, 17, 11u);
			F(ref B, C, D, A, 11u, 22, 12u);
			F(ref A, B, C, D, 12u, 7, 13u);
			F(ref D, A, B, C, 13u, 12, 14u);
			F(ref C, D, A, B, 14u, 17, 15u);
			F(ref B, C, D, A, 15u, 22, 16u);
			G(ref A, B, C, D, 1u, 5, 17u);
			G(ref D, A, B, C, 6u, 9, 18u);
			G(ref C, D, A, B, 11u, 14, 19u);
			G(ref B, C, D, A, 0u, 20, 20u);
			G(ref A, B, C, D, 5u, 5, 21u);
			G(ref D, A, B, C, 10u, 9, 22u);
			G(ref C, D, A, B, 15u, 14, 23u);
			G(ref B, C, D, A, 4u, 20, 24u);
			G(ref A, B, C, D, 9u, 5, 25u);
			G(ref D, A, B, C, 14u, 9, 26u);
			G(ref C, D, A, B, 3u, 14, 27u);
			G(ref B, C, D, A, 8u, 20, 28u);
			G(ref A, B, C, D, 13u, 5, 29u);
			G(ref D, A, B, C, 2u, 9, 30u);
			G(ref C, D, A, B, 7u, 14, 31u);
			G(ref B, C, D, A, 12u, 20, 32u);
			H(ref A, B, C, D, 5u, 4, 33u);
			H(ref D, A, B, C, 8u, 11, 34u);
			H(ref C, D, A, B, 11u, 16, 35u);
			H(ref B, C, D, A, 14u, 23, 36u);
			H(ref A, B, C, D, 1u, 4, 37u);
			H(ref D, A, B, C, 4u, 11, 38u);
			H(ref C, D, A, B, 7u, 16, 39u);
			H(ref B, C, D, A, 10u, 23, 40u);
			H(ref A, B, C, D, 13u, 4, 41u);
			H(ref D, A, B, C, 0u, 11, 42u);
			H(ref C, D, A, B, 3u, 16, 43u);
			H(ref B, C, D, A, 6u, 23, 44u);
			H(ref A, B, C, D, 9u, 4, 45u);
			H(ref D, A, B, C, 12u, 11, 46u);
			H(ref C, D, A, B, 15u, 16, 47u);
			H(ref B, C, D, A, 2u, 23, 48u);
			I(ref A, B, C, D, 0u, 6, 49u);
			I(ref D, A, B, C, 7u, 10, 50u);
			I(ref C, D, A, B, 14u, 15, 51u);
			I(ref B, C, D, A, 5u, 21, 52u);
			I(ref A, B, C, D, 12u, 6, 53u);
			I(ref D, A, B, C, 3u, 10, 54u);
			I(ref C, D, A, B, 10u, 15, 55u);
			I(ref B, C, D, A, 1u, 21, 56u);
			I(ref A, B, C, D, 8u, 6, 57u);
			I(ref D, A, B, C, 15u, 10, 58u);
			I(ref C, D, A, B, 6u, 15, 59u);
			I(ref B, C, D, A, 13u, 21, 60u);
			I(ref A, B, C, D, 4u, 6, 61u);
			I(ref D, A, B, C, 11u, 10, 62u);
			I(ref C, D, A, B, 2u, 15, 63u);
			I(ref B, C, D, A, 9u, 21, 64u);
			A += num;
			B += num2;
			C += num3;
			D += num4;
		}

		private byte[] CreatePaddedBuffer(byte[] mes)
		{
			uint num = (uint)((448 - mes.Length * 8 % 512 + 512) % 512);
			if (num == 0)
			{
				num = 512u;
			}
			uint num2 = (uint)(mes.Length + num / 8u + 8);
			ulong num3 = (ulong)mes.Length * 8uL;
			byte[] array = new byte[num2];
			for (int i = 0; i < mes.Length; i++)
			{
				array[i] = mes[i];
			}
			array[mes.Length] |= 128;
			for (int num4 = 8; num4 > 0; num4--)
			{
				array[num2 - num4] = (byte)((num3 >> (8 - num4) * 8) & 0xFF);
			}
			return array;
		}

		private void CopyBlock(byte[] bMsg, uint block)
		{
			block <<= 6;
			for (uint num = 0u; num < 61; num += 4)
			{
				X[num >> 2] = (uint)((bMsg[block + num + 3] << 24) | (bMsg[block + num + 2] << 16) | (bMsg[block + num + 1] << 8) | bMsg[block + num]);
			}
		}

		private void F(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
		{
			a = b + BitHelper.RotateLeft(a + ((b & c) | (~b & d)) + X[k] + T[i - 1], s);
		}

		private void G(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
		{
			a = b + BitHelper.RotateLeft(a + ((b & d) | (c & ~d)) + X[k] + T[i - 1], s);
		}

		private void H(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
		{
			a = b + BitHelper.RotateLeft(a + (b ^ c ^ d) + X[k] + T[i - 1], s);
		}

		private void I(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
		{
			a = b + BitHelper.RotateLeft(a + (c ^ (b | ~d)) + X[k] + T[i - 1], s);
		}
	}
}
