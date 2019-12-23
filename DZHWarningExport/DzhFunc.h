#ifndef __DZHFUNC_H_
#define __DZHFUNC_H_

#include<time.h>
#include "pch.h"
//���ǻ���չ�����淶V1.10
//1.���淶�����ڴ��ǻ�1.10��׼���רҵ�湫ʽϵͳ.
//2.��չ��������ʵ��ϵͳ��������ʵ�ֵ������㷨.
//3.��չ������windows 32λ��̬���ӿ�ʵ��, ����ʹ��Microsoft Visual C++���.
//4.����ʱ�ڹ�ʽ�༭����д"��̬������@��������"(������)����, �������溯������дΪ"DZHFUNC@MYCMALOSE"(5)
//5.��̬���ӿ����ƺͺ������ƿ����Լ�����.
//6.ʹ��ʱ���Խ���̬�⿽�������ǻ�Ŀ¼��ʹ��.

#ifdef __cplusplus
extern "C"
{
#endif //_cplusplus

	///////////////////////////////////////////////////////////////////////////
	//��������
	enum DATA_TYPE
	{
		TICK_DATA = 2, //�ֱʳɽ�
		MIN1_DATA, //1������
		MIN5_DATA, //5������
		MIN15_DATA, //15������
		MIN30_DATA, //30������
		MIN60_DATA, //60������
		DAY_DATA, //����
		WEEK_DATA, //����
		MONTH_DATA, //����
		MULTI_DATA //������
	};

	///////////////////////////////////////////////////////////////////////////
	//��������

	typedef struct tagSTKDATA
	{
		time_t m_time; //ʱ��,UCT
		float m_fOpen; //����
		float m_fHigh; //���
		float m_fLow; //���
		float m_fClose; //����
		float m_fVolume; //�ɽ���
		float m_fAmount; //�ɽ���
		WORD m_wAdvance; //���Ǽ���(��������Ч)
		WORD m_wDecline; //�µ�����(��������Ч)
	}STKDATA;


	////////////////////////////////////////////////////////////////////////////
	//��չ����,���������ֱʳɽ����ݵ�������

	typedef union tagSTKDATAEx
	{
		struct
		{
			float m_fBuyPrice[3]; //��1--��3��
			float m_fBuyVol[3]; //��1--��3��
			float m_fSellPrice[3]; //��1--��3��
			float m_fSellVol[3]; //��1--��3��
		};
		float m_fDataEx[12]; //����
	}STKDATAEx;

	/////////////////////////////////////////////////////////////////////////////


	/////////////////////////////////////////////////////////////////////////////
	//�������ݽṹ

	typedef struct tagCALCINFO
	{
		const DWORD m_dwSize; //�ṹ��С
		const DWORD m_dwVersion; //��������汾(V2.10 : 0x210)
		const DWORD m_dwSerial; //����������к�
		const char* m_strStkLabel; //��Ʊ����
		const BOOL m_bIndex; //����

		const int m_nNumData; //��������(pData,pDataEx,pResultBuf��������)
		const STKDATA* m_pData; //��������,ע��:��m_nNumData==0ʱ����Ϊ NULL
		const STKDATAEx* m_pDataEx; //��չ����,�ֱʳɽ�������,ע��:����Ϊ NULL

		//m_nParam1Start����ָ������1�ǳ��������������в���
		//��m_nParam1Start<0, �����1Ϊ��������,��������*m_pfParam1;
		//��m_nParam1Start>=0,�����1Ϊ����������,m_pfParam1ָ��һ������������,
		//�����СΪm_nNumData,������Ч��ΧΪm_nParam1Start--m_nNumData.
		//��ʱ����m_pData[x] �� m_pfParam1[x]��һ�µ�
		const int m_nParam1Start;

		//1.�������ò�����m_pfParam1--m_pfParam4����,��ΪNULL���ʾ�ò�����Ч.
		//2.��һ��������Чʱ,���������в�������Ч.
		// ��:m_pfParam2ΪNULL,��m_pfParam3,m_pfParam4һ��ΪNULL.
		//3.����1�����ǳ�������������������,�������ֻ��Ϊ��������.
		const float* m_pfParam1; //���ò���1
		const float* m_pfParam2; //���ò���2
		const float* m_pfParam3; //���ò���3
		const float* m_pfParam4; //���ò���4

		//������������pData->m_pResultBuf����.
		float* m_pResultBuf; //���������

		const DATA_TYPE m_dataType; //��������
		const float* m_pfFinData; //��������
	}CALCINFO;



	/////////////////////////////////////////////////////////////////////////


	//ʾ������,ʹ��ʱ��ʵ�������滻
	__declspec(dllexport) int WINAPI MYMACLOSE(CALCINFO* pData);
	__declspec(dllexport) int WINAPI MYMAVAR(CALCINFO* pData);

	__declspec(dllexport) int WINAPI MYMACLOSE_CALC_PREV(CALCINFO* pData);
	__declspec(dllexport) int WINAPI MYMAVAR_CALC_PREV(CALCINFO* pData);
	__declspec(dllexport) int WINAPI WRITETOTXT(CALCINFO* pData);

#ifdef __cplusplus
}
#endif //_cplusplus

#endif //_DZHFUNC_H_