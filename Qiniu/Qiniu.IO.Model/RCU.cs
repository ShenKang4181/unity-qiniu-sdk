namespace Qiniu.IO.Model
{
	public class RCU
	{
		private static int N = 131072;

		public static int GetChunkSize(ChunkUnit cu)
		{
			return (int)cu * N;
		}

		public static ChunkUnit GetChunkUnit(int chunkSize)
		{
			if (chunkSize < 131072 || chunkSize > 4194304)
			{
				return ChunkUnit.U2048K;
			}
			int num = chunkSize / N;
			if (num == 1)
			{
				return ChunkUnit.U128K;
			}
			if (num < 4)
			{
				return ChunkUnit.U256K;
			}
			if (num < 8)
			{
				return ChunkUnit.U512K;
			}
			if (num < 16)
			{
				return ChunkUnit.U1024K;
			}
			if (num < 32)
			{
				return ChunkUnit.U2048K;
			}
			return ChunkUnit.U4096K;
		}
	}
}
