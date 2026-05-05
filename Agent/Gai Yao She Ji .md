# 对话交互评分游戏 - 概要设计

---

## 一、系统总体架构

### 1.1 系统分层架构
<img width="1056" height="425" alt="屏幕截图 2026-05-05 194252" src="https://github.com/user-attachments/assets/d4dba872-4cc8-4db9-95f8-415cd8c861c1" />

<img width="1065" height="601" alt="屏幕截图 2026-05-05 194305" src="https://github.com/user-attachments/assets/4d99f242-8f43-4b5c-aea1-4ab53d796280" />

## 二、UML 设计
### 2.1 用例图
<img width="957" height="1049" alt="屏幕截图 2026-05-05 194324" src="https://github.com/user-attachments/assets/b9b22e85-98ab-4b8c-9d8e-2db019ea6687" />







### 2.2 活动图（业务流程）
<img width="1004" height="1233" alt="屏幕截图 2026-05-05 194410" src="https://github.com/user-attachments/assets/529d3de7-3363-4687-bc06-5749def01238" />





### 2.3 类图
<img width="1070" height="514" alt="屏幕截图 2026-05-05 194425" src="https://github.com/user-attachments/assets/d173b0e5-7807-4077-9e57-7cd05bad8953" />



### 2.4 时序图（对话评分流程）
<img width="1043" height="573" alt="屏幕截图 2026-05-05 194430" src="https://github.com/user-attachments/assets/4624bd20-f042-40b4-b56d-40eba5553a6f" />






### 2.5 对象协作图

<img width="1022" height="734" alt="屏幕截图 2026-05-05 194435" src="https://github.com/user-attachments/assets/6622f67f-2ec3-4cde-94ae-56fb79d9e8cd" />








## 三、数据库设计
### 3.1 ER 图
<img width="1033" height="1088" alt="屏幕截图 2026-05-05 194457" src="https://github.com/user-attachments/assets/228c4cf4-1a11-4dd3-8040-42375336382b" />



### 3.2关系数据模型
| 数据表名称 | 包含字段 |
| :--- | :--- |
| 用户表 | 用户ID、昵称、创建时间、是否删除 |
| 游戏场景表 | 场景ID、场景编码、场景名称、最大轮次、胜利分数、场景描述 |
| 游戏对局表 | 对局ID、用户ID、场景ID、总分数、当前轮次、是否结束、游戏结果 |
| 轮次对话表 | 对话ID、对局ID、轮次号、玩家消息、AI回复、发送时间 |
| 轮次评分表 | 评分ID、对话ID、本轮分数、评分规则、计算时间 |
| 评分规则表 | 规则ID、场景ID、规则类型、关键词、分值、规则描述 |
| AI交互记录表 | 请求ID、对局ID、请求内容、响应内容、请求状态、请求时间 |
### 3.3 对象 - 关系映射（ORM


#### 1. 用户类（User） ↔ 用户表（t_user）
- userId ↔ user_id
- userName ↔ user_name
- createTime ↔ create_time

#### 2. 游戏场景类（GameScene） ↔ 游戏场景表（t_game_scene）
- sceneId ↔ scene_id
- sceneName ↔ scene_name
- maxRound ↔ max_round
- winScore ↔ win_score

#### 3. 游戏对局类（GameRound） ↔ 游戏对局表（t_game_round）
- gameId ↔ game_id
- userId ↔ user_id
- sceneId ↔ scene_id
- totalScore ↔ total_score

#### 4. 对话类（Dialogue） ↔ 轮次对话表（t_dialogue）
- dialogueId ↔ dialogue_id
- gameId ↔ game_id
- playerMsg ↔ player_message
- aiReply ↔ ai_reply

#### 5. 评分类（RoundScore） ↔ 轮次评分表（t_round_score）
- scoreId ↔ score_id
- dialogueId ↔ dialogue_id
- roundScore ↔ round_score

#### 6. 评分规则类（ScoreRule） ↔ 评分规则表（t_score_rule）
- ruleId ↔ rule_id
- sceneId ↔ scene_id
- keyword ↔ keyword
- scoreValue ↔ score_value
