// VisionLibrary.h : VisionLibrary DLL�� �⺻ ��� �����Դϴ�.
//

#pragma once

#ifndef __AFXWIN_H__
	#error "PCH�� ���� �� ������ �����ϱ� ���� 'stdafx.h'�� �����մϴ�."
#endif

#include "resource.h"		// �� ��ȣ�Դϴ�.


// CVisionLibraryApp
// �� Ŭ������ ������ ������ VisionLibrary.cpp�� �����Ͻʽÿ�.
//

class CVisionLibraryApp : public CWinApp
{
public:
	CVisionLibraryApp();

// �������Դϴ�.
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
