# Unity_3dLog_SaveAndLoad

![](https://github.com/DevDiabloH/ImageSources/blob/master/3dLog/PreviewGif.gif)


간략히 기능을 보여주는 영상 입니다.


아래로는 설명입니다.


![](https://github.com/DevDiabloH/ImageSources/blob/master/3dLog/Preview.png)

1. 3d 메모 
     + 아이콘 생성 버튼

2. 모든 메모를 저장 
     + SaveLoadManager::GetBinaryFilePath()의 경로로 저장됩니다.
     + 각각의 Scene별로 메모를 저장하길 희망한다면, SaveLoadManager::fileName을 커스텀하여 사용하면 됩니다.

3. 모든 메모를 로드
     + 저장된 fileName.fileExtension을 불러옵니다.
     + 파일이 없거나, 메모가 0개 일 때 예외처리 됩니다.

4. 메모 저장
     + 8-1의 3d 메모 아이콘을 클릭하면, 숨겨져있던 ConfirmUI가 나타납니다.
     + 해당 버튼을 누르면 단일 메모가 저장됨과 동시에 2번의 모두 저장하기가 호출됩니다.

5. 메모 삭제
     + 메모를 삭제합니다.
     + 단, 이미 저장되어 있는 상태에서 삭제한 후 저장을 하지 않는다면 로드할 때 다시 불러올 수 있습니다.

7. 닫기
     + Confirm UI를 닫습니다.

8. 우선순위
     + 작업의 우선순위를 나타냅니다. +- 버튼을 통해 번호를 수정할 수 있습니다.
     + 작업 우선순위는 8-1 버튼에 텍스트로 표현됩니다.

9. 키워드
     + 작업자가 확인해야하는 특정 키워드를 나타냅니다.
     + 키워드는 최대 8개까지 생성할 수 있습니다.
     + Common::ConfirmKeyword의 Enum을 수정하여 키워드를 즉시 바꿀 수 있습니다.

10. 피드백 내용

     + 작업자가 어떠한 점을 수정해야하는지 알 수 있도록 텍스트를 입력하는 인풋필드 입니다.

11. 피드백 작성자
     + 피드백을 작성한 대상에 대한 정보를 나타냅니다.
     + Dropdown의 0번 아이템은 0 클라이언트로 사용합니다.
     + 피드백 출처가 클라이언트인 경우 12번의 컨펌 대상자는 2칸이 활성화되며,
     + 피드백 출처가 0번이 아닌 경우에는 12번의 컨펌 대상자는 1칸만 활성화 됩니다.

12. 피드백 완료 여부
     + 피드백 내용이 수정된 경우 해당자는 체크박스에 체크하여 완료 여부를 나타냅니다.

13. 드래그
     + 메모 아이콘에 보이는 막대기 부분은 드래그하여 **world**에 위치를 재배치 할 수 있습니다.
     + 좌측의 Confirm UI 역시 검은색 부분을 드래그하여 **2d**상 위치를 재배치 할 수 있습니다.
     
     + ConfirmUI(2d)는 사용자의 해상도에 따라 최대/최소 범위를 벗어날 수 없도록 개발되었습니다.
     ![](https://github.com/DevDiabloH/ImageSources/blob/master/3dLog/LimitPosition.gif)


