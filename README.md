# 🌿 Foret

**장르**: 2D 메트로배니아  
**엔진**: Unity 2022.3.17f1  
**플랫폼**: PC  
**프로그래밍 언어**: C#

---

## 📖 게임 개요

과거 기사단의 일원으로 마왕과 싸웠던 기사가 고향으로 돌아옵니다. 그로부터 5년 후, 갑자기 숲의 동물은 흉폭해진다. 그리고 그 안은 더 오염되어 괴물들이 넘쳐 흐르는데...

---

## 🗂️ 프로젝트 구조

### 🔧 핵심 시스템

| 스크립트 | 설명 |
|----------|------|
| `GameManager.cs` | 전역 상태 관리 (플레이어 체력, 리스폰 위치, 진행도 저장) |
| `UIManager.cs` | 게임 UI 컨트롤러 관리 및 이벤트 처리 |
| `AudioManager.cs` | 씬 별 BGM 자동 처리 및 슬라이더 기반 볼륨 제어 |
| `PoolManager.cs` | 투사체 및 몬스터 풀링 시스템 |

---

### 🧠 FSM / 애니메이션

| 디렉토리 | 설명 |
|----------|------|
| `Animator/EndParamBehaviour.cs` | 상태 종료 시 애니메이션 파라미터 초기화 |
| `Enemy/BossAnimationHandler.cs` | 보스의 FSM 상태 변화에 따라 애니메이션 실행 |
| `Effect/EffectAnimator.cs` | 이펙트 오브젝트 활성화 시 1회성 애니메이션 재생 |

---

### 🧟‍♀️ 적 & 보스

| 디렉토리 | 주요 스크립트 |
|----------|----------------|
| `Enemy/Reaper.cs` | 최종 보스 ‘진 마왕’의 AI 및 공격 패턴 |
| `Enemy/FGAnimationEvent.cs` | 보스 숲의 수호자 - 공격 히트박스 및 상태 이벤트 처리 |
| `Enemy/EnemyAttackHitbox.cs` | 충돌 기반 피해 처리, IDamagable 인터페이스 연동 |
| `Enemy/DropAttackHandler.cs` | 낙하형 광역기 패턴 처리 |

---

### 💬 상호작용 & NPC

| 디렉토리 | 설명 |
|----------|------|
| `NPC/DialogueNPC.cs` | 대화, 인터랙션을 위한 베이스 NPC 클래스 |
| `Interact/Interactable.cs` | 상호작용 가능한 오브젝트 베이스 |
| `UI/DialogueController.cs` | 대사 UI 출력 및 말풍선 처리, 타이핑 효과 포함 |

---

### 💾 저장 및 로딩

| 디렉토리 | 설명 |
|----------|------|
| `Save/SaveLoadManager.cs` | 게임 저장/불러오기 로직 처리 |
| `Editor/SaveLoadManagerEditor.cs` | 저장 데이터 위치 및 삭제 기능 인스펙터 확장 |

---

### ⚔️ 플레이어 관련

| 디렉토리 | 설명 |
|----------|------|
| `Player/PlayerController.cs` | 이동, 점프, 공격 입력 처리 |
| `Player/PlayerStat.cs` | 체력, 에너지 등 플레이어 상태 저장 |
| `Projectile/Projectile.cs` | 원거리 공격용 투사체 처리 및 충돌 판정 |

---

## 🛠️ 주요 기능 요약

### 🎵 오디오 시스템
- BGM 및 효과음은 `AudioMixer`를 통해 조절 가능
- 씬 전환 시 자동으로 해당 BGM 재생 (페이드 인/아웃 포함)
- 슬라이더 연동 UI 지원

### 💬 대화 시스템
- NPC와의 대화는 ScriptableObject 또는 큐 기반으로 동작
- 대화 출력 중 타이핑 효과, 줄바꿈, 즉시 출력 기능 구현

### 💀 보스 AI
- 각 보스는 FSM 기반으로 패턴 전환
- 차징, 순간이동, 분신, 광역기 등 다양한 공격 방식
- 피격 시 상태 전환 및 무적 판정 처리

### 📦 오브젝트 풀링
- `ProjectilePool`, `MonsterPool`을 통한 리소스 최적화
- 런타임 중 오브젝트 생성 최소화

### 💾 저장 시스템
- 인게임 저장 자동 처리 (`GameManager.OnApplicationQuit`)
- 세이브 에디터 확장 제공: 저장 경로 확인, 삭제 버튼 제공

---

## 🙌 크레딧

| 역할 | 이름 |
|------|------|
| 기획 & 개발 | [13시 밥먹기조] |
| 그래픽 & 사운드 | TBD |


