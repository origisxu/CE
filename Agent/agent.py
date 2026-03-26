import os
import shutil
import json
import requests
import subprocess
import re
from openai import OpenAI

# ⚠️ 请在此处填入您的 DeepSeek API Key
API_KEY = "sk-f3f81de4dba44177be847c1dbad6f7dc" 

# 初始化客户端
client = OpenAI(api_key=API_KEY, base_url="https://api.deepseek.com")

def create_directory(path):
    """创建一个新目录"""
    try:
        os.makedirs(path, exist_ok=True)
        return f"✅ 成功创建目录: {path}"
    except Exception as e:
        return f"❌ 创建目录失败: {str(e)}"

def move_file_to_folder(file_path, folder_path):
    """将文件移动到指定文件夹"""
    try:
        if not os.path.exists(folder_path):
            os.makedirs(folder_path, exist_ok=True)
        filename = os.path.basename(file_path)
        dest_path = os.path.join(folder_path, filename)
        shutil.move(file_path, dest_path)
        return f"✅ 成功将 '{filename}' 移动到 '{folder_path}'"
    except Exception as e:
        return f"❌ 移动文件失败: {str(e)}"

def get_weather(city):
    """查询指定城市的天气"""
    try:
        url = f"https://wttr.in/{city}?format=%C+%t"
        response = requests.get(url, timeout=5)
        if response.status_code == 200:
            return f"🌤️ {city} 的天气情况: {response.text.strip()}"
        else:
            return f"❌ 无法获取 {city} 的天气信息"
    except Exception as e:
        return f"❌ 天气查询出错: {str(e)}"

def run_shell_command(command):
    """
    执行系统命令 (增强版：自动修正 Linux 命令 + 适配 Windows 编码)
    """
    original_command = command
    
    # 即使 AI 犯了错发了 Linux 命令，这里也会自动把它改成 Windows CMD 命令
    cmd_lower = command.lower().strip()
    
    # 常见 Linux 命令 -> Windows CMD 命令 映射
    if cmd_lower.startswith('ls'):
        # ls, ls -l, ls -la 全部转为 dir
        command = 'dir'
        print(f"   🔄 [系统自动修正] 检测到 'ls'，已自动转换为 'dir'")
        
    elif cmd_lower.startswith('cat '):
        # cat file.txt -> type file.txt
        command = 'type ' + command[4:]
        print(f"   🔄 [系统自动修正] 检测到 'cat'，已自动转换为 'type'")
        
    elif cmd_lower.startswith('rm '):
        # rm file.txt -> del file.txt
        command = 'del ' + command[3:]
        print(f"   🔄 [系统自动修正] 检测到 'rm'，已自动转换为 'del'")
        
    elif cmd_lower.startswith('mv '):
        # mv a b -> move a b
        command = 'move ' + command[3:]
        print(f"   🔄 [系统自动修正] 检测到 'mv'，已自动转换为 'move'")
        
    elif cmd_lower.startswith('cp '):
        # cp a b -> copy a b
        command = 'copy ' + command[3:]
        print(f"   🔄 [系统自动修正] 检测到 'cp'，已自动转换为 'copy'")
        
    elif cmd_lower == 'pwd':
        command = 'cd'
        print(f"   🔄 [系统自动修正] 检测到 'pwd'，已自动转换为 'cd'")
        
    elif cmd_lower.startswith('grep '):
        # grep "text" file -> findstr "text" file
        command = 'findstr ' + command[5:]
        print(f"   🔄 [系统自动修正] 检测到 'grep'，已自动转换为 'findstr'")

    # === 安全检查 ===
    dangerous_patterns = [r'format\s', r'del\s+\/[fq]', r'shutdown', r'system32']
    for pattern in dangerous_patterns:
        if re.search(pattern, command.lower()):
            return f"⚠️ 安全拦截：危险操作 '{command}' 被拒绝。"

    try:
        # 关键点：encoding='gbk' 适配 Windows CMD 默认编码，解决乱码问题
        result = subprocess.run(
            command,
            shell=True,
            capture_output=True,
            text=True,
            timeout=15,
            encoding='gbk', 
            errors='ignore'
        )
        
        output = result.stdout
        error = result.stderr
        
        response_parts = []
        
        if output:
            # 限制输出长度
            if len(output) > 3000:
                output = output[:3000] + "\n... (输出过长，已截断)"
            response_parts.append(f"💻 命令输出:\n{output}")
        
        if error:
            # 如果有错误，返回给 AI 让它知道
            response_parts.append(f"⚠️ 错误信息:\n{error}")
            
        if not output and not error:
            return "✅ 命令执行成功，无输出。"
            
        return "\n".join(response_parts)
            
    except subprocess.TimeoutExpired:
        return "⚠️ 错误：命令执行超时 (超过15秒)。"
    except Exception as e:
        return f"❌ 系统异常: {str(e)}"

def list_files():
    """列出当前目录的文件（Python 原生实现，最稳定）"""
    try:
        files = os.listdir('.')
        result = "📁 当前目录的文件列表:\n"
        for f in files:
            if os.path.isdir(f):
                result += f"  📂 {f}/\n"
            else:
                size = os.path.getsize(f)
                result += f"  📄 {f} ({size} 字节)\n"
        return result
    except Exception as e:
        return f"❌ 列出文件失败: {str(e)}"

tools = [
    {
        "type": "function",
        "function": {
            "name": "create_directory",
            "description": "当用户需要创建新文件夹时使用",
            "parameters": {
                "type": "object",
                "properties": {
                    "path": {"type": "string", "description": "要创建的文件夹路径"}
                },
                "required": ["path"]
            }
        }
    },
    {
        "type": "function",
        "function": {
            "name": "move_file_to_folder",
            "description": "当用户需要整理或移动文件时使用",
            "parameters": {
                "type": "object",
                "properties": {
                    "file_path": {"type": "string", "description": "源文件路径"},
                    "folder_path": {"type": "string", "description": "目标文件夹路径"}
                },
                "required": ["file_path", "folder_path"]
            }
        }
    },
    {
        "type": "function",
        "function": {
            "name": "get_weather",
            "description": "当用户询问天气情况时使用",
            "parameters": {
                "type": "object",
                "properties": {
                    "city": {"type": "string", "description": "城市名称，如 'Beijing' 或 'Shanghai'"}
                },
                "required": ["city"]
            }
        }
    },
    {
        "type": "function",
        "function": {
            "name": "run_shell_command",
            "description": "当用户需要执行系统命令、查看目录列表或进行简单计算时使用。注意：环境是 Windows CMD，严禁使用 Linux 命令。",
            "parameters": {
                "type": "object",
                "properties": {
                    "command": {"type": "string", "description": "要执行的命令行指令 (例如: 'dir', 'type file.txt', 'mkdir test')"}
                },
                "required": ["command"]
            }
        }
    },
    {
        "type": "function",
        "function": {
            "name": "list_files",
            "description": "列出当前目录的所有文件和文件夹（专门用于查看文件列表，比 shell 命令更稳定）",
            "parameters": {
                "type": "object",
                "properties": {}
            }
        }
    }
]

available_functions = {
    "create_directory": create_directory,
    "move_file_to_folder": move_file_to_folder,
    "get_weather": get_weather,
    "run_shell_command": run_shell_command,
    "list_files": list_files,
}

def clean_deepseek_response(text):
    """清理 DeepSeek 可能返回的 XML 格式或无关字符"""
    if text in ["🔧 正在执行操作...", "⚙️ 正在执行操作..."]:
        return "操作已完成。"
    
    if '<｜DSML｜' in text or '<<tr>' in text:
        return "操作已完成。"
    
    return text

def run_agent():
    print("=" * 50)
    print("🤖 全能智能体已启动 (Windows CMD 优化版)!")
    print("支持功能：文件管理、天气查询、命令执行")
    print("特性：自动修正 Linux 命令，防止报错")
    print("=" * 50)
    print("💡 试试说：")
    print("   • '列出当前目录的所有文件'")
    print("   • '读取 agent.py 的前 5 行'")
    print("   • '北京天气怎么样'")
    print("   • '帮我在当前目录创建个测试文件夹'")
    print("=" * 50)
    
    #System Prompt
    messages = [
        {"role": "system", "content": """
【核心身份】
你是一个运行在 **纯 Windows CMD (cmd.exe)** 环境下的本地文件管理助手。
**重要前提**：当前环境**没有安装** Git Bash, WSL, Cygwin 或任何 Linux 模拟器。
**绝对事实**：在此环境中，输入 `ls`, `cat`, `rm`, `mv`, `grep`, `pwd` 等 Linux 命令会**直接报错**。

【命令映射表 (必须严格遵守)】
当你想要执行以下操作时，**必须**转换为右侧的 CMD 命令：
- ❌ `ls` / `ll`          -->  ✅ `dir`
- ❌ `cat` / `head`       -->  ✅ `type`
- ❌ `rm`                 -->  ✅ `del`
- ❌ `mv`                 -->  ✅ `move`
- ❌ `cp`                 -->  ✅ `copy`
- ❌ `pwd`                -->  ✅ `cd`
- ❌ `grep`               -->  ✅ `findstr`

【思维链要求】
1. 检查：我要用的命令是 Linux 风格的吗？
2. 转换：如果是，立即查表转换为 CMD 风格。
3. 执行：只输出转换后的 CMD 命令。

【错误处理】
- 如果之前的尝试返回了错误，**唯一原因**是你使用了错误的命令类型。
- 补救：立即将命令替换为上述映射表中的 CMD 等价物重试。

【工具策略】
- 查看文件列表：优先调用 `list_files` 工具。
- 读取文件内容：优先尝试用 `type` 命令。
- 输出结果：直接回答用户，不要复述命令细节，除非出错了。
"""}
    ]

    while True:
        user_input = input("\n👤 你: ").strip()
        if user_input.lower() in ['exit', 'quit', '退出']:
            print("👋 再见！")
            break
        
        if not user_input:
            continue
        
        messages.append({"role": "user", "content": user_input})

        try:
            response = client.chat.completions.create(
                model="deepseek-chat",
                messages=messages,
                tools=tools,
                tool_choice="auto",
                temperature=0.3
            )
            
            response_message = response.choices[0].message
            tool_calls = response_message.tool_calls

            if tool_calls:
                print("⚙️ 正在执行操作...")
                messages.append(response_message)

                for tool_call in tool_calls:
                    function_name = tool_call.function.name
                    function_to_call = available_functions[function_name]
                    function_args = json.loads(tool_call.function.arguments)

                    print(f"   🔧 调用: {function_name}")
                    if function_args:
                        # 隐藏部分敏感参数显示，保持整洁
                        args_str = str(function_args)
                        print(f"   📝 参数: {args_str[:100]}...")
                    
                    function_response = function_to_call(**function_args)
                    
                    # 打印结果预览
                    preview = function_response[:100] + "..." if len(function_response) > 100 else function_response
                    print(f"   ✅ 结果: {preview}")
                    
                    messages.append({
                        "tool_call_id": tool_call.id,
                        "role": "tool",
                        "name": function_name,
                        "content": function_response,
                    })

                second_response = client.chat.completions.create(
                    model="deepseek-chat",
                    messages=messages,
                    temperature=0.3
                )
                final_reply = second_response.choices[0].message.content
                final_reply = clean_deepseek_response(final_reply)
                
                print(f"\n🤖 AI: {final_reply}")
                messages.append({"role": "assistant", "content": final_reply})
            
            else:
                reply = response_message.content
                reply = clean_deepseek_response(reply)
                print(f"\n🤖 AI: {reply}")
                messages.append(response_message)
                
        except Exception as e:
            print(f"\n❌ 发生错误: {str(e)}")
            # 出错时移除最后一条用户消息，避免污染上下文
            if len(messages) > 1:
                messages.pop()

if __name__ == "__main__":
    run_agent()