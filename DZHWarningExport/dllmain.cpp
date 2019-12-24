
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
//write2txt : = "DZHWarningExport@WRITETOTXT"(0,DYNAINFO(7)* 条件, DYNAINFO(14) * 条件, vol * 条件);{0 buy,1 sell,除了第一个参数，必须 *条件}
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
    string WarningTitle;
    if (*pData->m_pfParam1 == 0) {
        WarningTitle = "BUY";
    }else if (*pData->m_pfParam1 == 1) {
        WarningTitle = "SELL";
    }

    string TodayTxtName = folderPath + "/" + Year + Month + Day + ".txt";
    if (_access(TodayTxtName.c_str(), 0) == -1)
    {
        ofstream fout(TodayTxtName);
    }

    ofstream write(TodayTxtName, ios::app);//打开record.txt文件，以ios::app追加的方式输入
    //603855	华荣股份	2019-12-23 16:39	10.13	 0.10%	  871	条件名称	
    write << pData->m_strStkLabel << "\t" 
        << "股票名称\t" << tmp << +"\t" 
        << *pData->m_pfParam2<<"\t" 
        << doubleRound(*pData->m_pfParam3*100,2)<<"%\t"
        << *pData->m_pfParam4 << "\t" 
        << WarningTitle << endl;//数据写入文件
    write.close();//关闭文件

    return -1;
}
