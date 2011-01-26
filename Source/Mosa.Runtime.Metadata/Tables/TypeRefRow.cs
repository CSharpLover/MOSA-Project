/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Runtime.Metadata.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public struct TypeRefRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes resolutionScopeIdx;

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes typeNameIdx;

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes typeNamespaceIdx;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeRefRow"/> struct.
		/// </summary>
		/// <param name="resolutionScopeIdx">The resolution scope idx.</param>
		/// <param name="typeNameIdx">The type name idx.</param>
		/// <param name="typeNamespaceIdx">The type namespace idx.</param>
		public TypeRefRow(TokenTypes resolutionScopeIdx, TokenTypes typeNameIdx, TokenTypes typeNamespaceIdx)
		{
			this.resolutionScopeIdx = resolutionScopeIdx;
			this.typeNameIdx = typeNameIdx;
			this.typeNamespaceIdx = typeNamespaceIdx;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets or sets the resolution scope idx.
		/// </summary>
		/// <value>The resolution scope idx.</value>
		public TokenTypes ResolutionScopeIdx
		{
			get { return resolutionScopeIdx; }
			set { resolutionScopeIdx = value; }
		}

		/// <summary>
		/// Gets or sets the type name idx.
		/// </summary>
		/// <value>The type name idx.</value>
		public TokenTypes TypeNameIdx
		{
			get { return typeNameIdx; }
			set { typeNameIdx = value; }
		}

		/// <summary>
		/// Gets or sets the type namespace idx.
		/// </summary>
		/// <value>The type namespace idx.</value>
		public TokenTypes TypeNamespaceIdx
		{
			get { return typeNamespaceIdx; }
			set { typeNamespaceIdx = value; }
		}

		#endregion // Properties
	}
}
