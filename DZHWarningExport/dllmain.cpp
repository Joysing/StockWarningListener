
// dllmain.cpp : 定义 DLL 应用程序的入口点。

#include "pch.h"
#include "DzhFunc.h"
#include <fstream>
#include<io.h>
#include<direct.h> 
#include <ctime>
#include<string>
#include <iostream>

using namespace std;


//大智慧调用：
//
//条件: OPEN > 200;
//IF 条件 THEN
//BEGIN
//write2txt : = "DZHWarningExport@WRITETOTXT"(DYNAINFO(7), DYNAINFO(14) * 条件, vol * 条件);{除了第一个参数，必须 *条件}
//END;
string folderPath = "WarningTxt";
LPCWSTR stringToLPCWSTR(std::string orig)
{
    size_t origsize = orig.length() + 1;
    const size_t newsize = 100;
    size_t convertedChars = 0;
    wchar_t* wcstring = (wchar_t*)malloc(sizeof(wchar_t) * (orig.length() - 1));
    mbstowcs_s(&convertedChars, wcstring, origsize, orig.c_str(), _TRUNCATE);

    return wcstring;
}

//四舍五入
double doubleRound(double number, int bits) //number->浮点数，bits->保留位数
{
    for (int i = 0;i < bits;++i)
    {
        number *= 10;
    }
    if (number > 0)
        number = (long long)(number + 0.5);
    else if (number < 0)
        number = (long long)(number - 0.5);

    for (int i = 0;i < bits;++i)
    {
        number /= 10;
    }
    return number;
}
BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
        case DLL_PROCESS_ATTACH:
        case DLL_THREAD_ATTACH:
        case DLL_THREAD_DETACH:
        case DLL_PROCESS_DETACH:
            break;
    }

    if (_access(folderPath.c_str(), 0) == -1)
    {
        int result=_mkdir(folderPath.c_str());
    }
    
    return TRUE;
}

//计算收盘价的均价,一个常数参数,表示计算周期
  //调用时在公式编辑器中写"动态库名称@函数名称"(参数表),
  //例如:"mydzhdll@mymaclose"(10)



  //---------------------------- 函数 MA(N)--------------------------
  //这是一个求N日收盘平均价的函数，参数N将从m_pfParam1中传入
  //用法:MA(N),求某量X的N日简单移动平均值。
  //算法：(X1+X2+X3+...+Xn)/N
  //例如:MA(20)表示求20日均价
  //求哪个量的N日平均？结构指针STKDATA* m_pData的成员m_fClose。
  //使用方式 "MyDzhDll@MYMACLOSE"(5)
  //-----------------------------函数源码----------------------------

__declspec(dllexport) int WINAPI MYMACLOSE(CALCINFO* pData)
{
    float f, fTotal;
    int nPeriod, i, j;
    if (pData->m_pfParam1 &&//参数1有效
        pData->m_nParam1Start < 0 &&//参数1为常数
        pData->m_pfParam2 == NULL)//仅有一个参数
    {
        f = *pData->m_pfParam1;
        nPeriod = (int)f;//参数1
        if (nPeriod > 0)
        {
            //计算nPeriod周期的均线,数据从nPeriod-1开始有效
            for (i = nPeriod - 1;i<pData->m_nNumData;i++)
            {
                fTotal = 0.0f;
                for (j = 0;j<nPeriod;j++)
                    fTotal += pData->m_pData[i - j].m_fClose;

                    //函数计算结果用pData->m_pResultBuf带回.
                    pData->m_pResultBuf[i] = fTotal / nPeriod;//平均
            }
            return nPeriod - 1;
        }
    }
    return -1;
}

//这个函数不知道干啥的，原模板带的；似乎无意义
__declspec(dllexport) int WINAPI MYMACLOSE_CALC_PREV(CALCINFO* pData)
{
    if (pData->m_pfParam1 && pData->m_nParam1Start < 0)
    {
        float f = *pData->m_pfParam1;
        return ((int)f) - 1;
    }
    return 0;
}

//---------------------------- 函数 MA(X,N)--------------------------
//求简单移动平均。
//用法:MA(X,N),求X的N日简单移动平均值。
//算法：(X1+X2+X3+...+Xn)/N
//例如:MA(CLOSE,10)表示求10日均价

//使用方式 "mydzhdll@mymavar"(C,5); 参数和系统自带的MA函数一样
//又如:"mydzhdll@mymavar"(CLOSE-OPEN,5)
//计算均价,2个参数,参数1为待求均线的数据,参数2表示计算周期
//------------------------------函数源码-----------------------------
__declspec(dllexport) int WINAPI MYMAVAR(CALCINFO* pData)
{
    float f, fTotal;
    const float* pValue;
    int nPeriod, nFirst, i, j;
    if (pData->m_pfParam1 && pData->m_pfParam2 && //参数1,2有效
        pData->m_nParam1Start >= 0 &&//参数1为序列数
        pData->m_pfParam3 == NULL)//有2个参数
    {
        pValue = pData->m_pfParam1;//参数1
        nFirst = pData->m_nParam1Start;//有效值
        f = *pData->m_pfParam2;
        nPeriod = (int)f;//参数2

        if (nFirst >= 0 && nPeriod > 0)
        {
            for (i = nFirst + nPeriod - 1;i<pData->m_nNumData;i++)
            {
                fTotal = 0.0f;
                for (j = 0;j<nPeriod;j++)
                    fTotal += pValue[i - j];

                    //函数计算结果用pData->m_pResultBuf带回.
                    pData->m_pResultBuf[i] = fTotal / nPeriod;//平均
            }
            return nFirst + nPeriod - 1;
        }
    }
    return -1;
}

//这个函数不知道干啥的，原模板带的；
__declspec(dllexport) int WINAPI MYMAVAR_CALC_PREV(CALCINFO* pData)
{
    if (pData->m_pfParam2)
    {
        float f = *pData->m_pfParam2;
        return ((int)f) - 1;
    }
    return 0;
}

__declspec(dllexport) int WINAPI WRITETOTXT(CALCINFO* pData)
{
    
    // 基于当前系统的当前日期/时间
    time_t now = time(0);
    tm* ltm = localtime(&now);
    // 输出 tm 结构的各个组成部分
    string Year = to_string(1900 + ltm->tm_year);
    string Month = to_string(1 + ltm->tm_mon);
    string Day = to_string(ltm->tm_mday);
    string Time = to_string(ltm->tm_hour) +":"+ to_string(ltm->tm_min) + ":" + to_string(ltm->tm_sec);
    char tmp[64];
    strftime(tmp, sizeof(tmp), "%Y-%m-%d %H:%M", ltm);
    string TodayTxtName = folderPath + "/" + Year + Month + Day + ".txt";
    if (_access(TodayTxtName.c_str(), 0) == -1)
    {
        ofstream fout(TodayTxtName);
    }

    ofstream write(TodayTxtName, ios::app);//打开record.txt文件，以ios::app追加的方式输入
    //603855	华荣股份	2019-12-23 16:39	10.13	 0.10%	  871	连涨数天	
    write << pData->m_strStkLabel << "\t" 
        << "股票名称\t" << tmp << +"\t" 
        << *pData->m_pfParam1<<"\t" 
        << doubleRound(*pData->m_pfParam2*100,2)<<"%\t"
        << *pData->m_pfParam3 << "\t" 
        << "预警条件" << endl;//数据写入文件
    write.close();//关闭文件

    return -1;
}
