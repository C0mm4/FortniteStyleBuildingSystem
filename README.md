# 📌 Fortnite 스타일 건축 시스템

## 1. 모듈 개요 (Overview)  
- **설명**: 포트나이트 스타일 건축 시스템
- **지원 Unity 버전**: 2022.3 LTS 이상  
- **의존성**: Unity new Input System - 플레이어 이동 및 건축 입력 관련

### 해당 코드는 데모만을 담은 내용이며, 실제 프로젝트에 적용된 내용은 https://github.com/khh7052/TeamSurvival 에서 확인 가능 
### ReadMe 에서의 데모 GIF은 실제 프로젝트에서 사용된 예시를 사용 
---

## 2. 키 입력 (Key Bindings)  

| 키(Key) | 동작(Action)        | 설명(Description)             |
|---------|---------------------|--------------------------------|
| **W**   | Move Forward        | 캐릭터를 앞쪽으로 이동 |
| **A**   | Move Left           | 캐릭터를 왼쪽으로 이동 |
| **S**   | Move Backward       | 캐릭터를 뒤쪽으로 이동 |
| **D**   | Move Right          | 캐릭터를 오른쪽으로 이동 |
| **Mouse**   | Camera Rotation          | 캐릭터가 보는 방향 조절 |
| **Space** | Jump               | 캐릭터가 점프 |
| **B** | BuildMode           | 건축 모드 On/Off |
| **1** | BuildMode-Floor           | 바닥 건축 모드 |
| **2** | BuildMode-Wall           | 벽 건축 모드 |
| **F** | Try Build           | 건축물 건축 |

---

## 3. 적용 방법 (Usage / Setup)  
### GameObject Inspector 연결 방식  
1. Player 오브젝트에 Player Input, Player, PlayerController, BuildingMode 연결
2. Player Input의 Behavior를 Invoke Unity Event로 설정 후 Action 및 Map 설정
3. Player의 Input Event 연결
   a. Move > PlayerController.OnMove
   b. Look > PlayerController.OnLook
   c. Jump > PlayerController.OnJump
   d. BuildMode > PlayerController.OnBuildButton
   e. TryBuild > PlayerController.OnBuildTryButton
   f. OnBuildSelect > BuildingMode.OnSelectMode 
5. PlayerController의 CameraContainer 연결 및 Min,Max X Look 값 지정 (Default -85, 85)
6. BuildingMode의 Check Rate, Ray Distance 설정 (Default 0.05, 2)
7. BuildMask, Buildable Layer 설정
     BuildMask : 건축 가능 영역 Ray 체크용 레이어 (Default : Ground, BuildObj, BuildableLayer)
     Buildable Layer : 건축 가능 영역 레이어 (Default : Ground, BuildObj)
8. Invisible Layer 추가
     (공중에서 건축 체크 시 Ray 감지가 되지 않아 허공에 건축 체크 확인용 투명 Collider Layer)

---

## 4. 주요 기능 (Features)  
-  B 키를 누르면 건축 모드 On/Off 토글
-  건축 모드 진입 시 1, 2번을 눌러 건축할 객체 선택 (바닥, 벽)
-  플레이어 이동과 마우스로 카메라를 조작하여 건축하고자 하는 영역 지정
-  건축 가능 영역은 초록색으로, 건축 불가능 영역은 빨간색으로 표시
-  F키를 누르면 해당 영역에 건축

**데모**  
인게임에서 건축 데모 (데모 용으로 건축 시 재료 소모를 제거해놓은 상태)

![2025-09-14 21-42-28](https://github.com/user-attachments/assets/1cbf58ef-4c53-4a4e-8275-6783061e8727)

건축 불가능한 영역

![2025-09-14 21-42-28 (1)](https://github.com/user-attachments/assets/62697a73-7bbe-4176-83dc-06dad3fbd42a)


---
