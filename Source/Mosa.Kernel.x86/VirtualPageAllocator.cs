﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86.Helpers;
using Mosa.Platform.Internal.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// A virtual page allocator.
	/// </summary>
	/// <remarks>
	/// This is a simple bitmap implementation with no optimizations.
	/// </remarks>
	public static class VirtualPageAllocator
	{
		private static uint pages;
		private static bool initialized = false;

		/// <summary>
		/// Setups this instance.
		/// </summary>
		public static void Setup()
		{
			pages = (PageFrameAllocator.TotalPages - Address.ReserveMemory) / PageFrameAllocator.PageSize;

			// Bits: 0 = Available, 1 = Not Available
			Memory.Clear(Address.VirtualPageAllocator, pages / 8);
			initialized = true;
		}

		/// <summary>
		/// Gets the index of the page.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		private static unsafe uint GetPageIndex(void* address)
		{
			return (((uint)address) - Address.ReserveMemory) / PageFrameAllocator.PageSize;
		}

		/// <summary>
		/// Sets the page status in the bitmap.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="free">if set to <c>true</c> [free].</param>
		private static void SetPageStatus(uint page, bool free)
		{
			uint at = Address.VirtualPageAllocator + (page / 32);
			byte bit = (byte)(page % 32);
			uint mask = (byte)(1 << bit);

			uint value = Native.Get32(at);

			if (free)
				value = value & ~mask;
			else
				value = value | mask;

			Native.Set32(at, value);
		}

		/// <summary>
		/// Gets the page status from the bitmap
		/// </summary>
		/// <param name="page">The page.</param>
		/// <returns></returns>
		private static bool GetPageStatus(uint page)  // true = available
		{
			uint at = (Address.VirtualPageAllocator + (page / 8));
			byte bit = (byte)(page % 8);
			byte mask = (byte)(1 << bit);

			byte value = Native.Get8(at);

			return (value & mask) == 0;
		}

		/// <summary>
		/// Reserves the pages.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public static uint Reserve(uint size)
		{
			Assert.True(initialized, "VirtualPageAllocator is not initialized");

			uint first = 0xFFFFFFFF; // Marker
			uint requested = ((size - 1) / PageFrameAllocator.PageSize) + 1;

			for (uint at = 0; at < pages; at++)
			{
				if (GetPageStatus(at))
				{
					if (first == 0xFFFFFFFF)
						first = at;

					if (at - first == requested)
					{
						for (uint index = 0; index < requested; index++)
							SetPageStatus(first + index, false);

						return ((first * PageFrameAllocator.PageSize) + Address.ReserveMemory);
					}
				}
				else
				{
					first = 0xFFFFFFFF;
				}
			}
			Assert.True(false, "VirtualPageAllocator cannot reserve memory");
			return 0;
		}

		/// <summary>
		/// Releases the pages.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="count">The count.</param>
		public static unsafe void Release(uint address, uint count)
		{
			uint start = GetPageIndex((void*)address);
			for (uint index = 0; index < count; index++)
				SetPageStatus(start + index, true);
		}
	}
}
