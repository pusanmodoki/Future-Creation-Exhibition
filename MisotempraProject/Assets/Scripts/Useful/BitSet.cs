using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BitSet
{
	public bool this[int index] { get { return m_bitSet[index]; } set { m_bitSet[index] = value; } }

	public BitSet(int size)
	{
		m_bitSet = new bool[size];
	}
	public BitSet(int size, bool init = false)
	{
		m_bitSet = new bool[size];
		for (int i = 0; i < size; ++i) m_bitSet[i] = init;
	}
	public BitSet(BitSet copy)
	{
		m_bitSet = new bool[copy.m_bitSet.Length];
		for (int i = 0; i < copy.m_bitSet.Length; ++i) m_bitSet[i] = copy.m_bitSet[i];
	}
	public BitSet(params bool[] inits)
	{
		m_bitSet = new bool[inits.Length];
		for (int i = 0; i < inits.Length; ++i) m_bitSet[i] = inits[i];
	}

	[SerializeField]
	bool[] m_bitSet;
}
