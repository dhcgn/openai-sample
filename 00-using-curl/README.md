# Using curl

## Prerequisites

```sh
OPENAI_API_KEY=sk-...
```

## First Try

### Simple

```sh
curl https://api.openai.com/v1/chat/completions   -H "Content-Type: application/json"   -H "Authorization: Bearer $OPENAI_API_KEY"   -d '{
  "model": "gpt-3.5-turbo",
  "messages": [
    {
      "role": "assistant",
      "content": "Tell my a 10 jokes about software developer or system admins"
    }
  ],
  "max_tokens": 256
}'
```

### Full

```sh
curl https://api.openai.com/v1/chat/completions   -H "Content-Type: application/json"   -H "Authorization: Bearer $OPENAI_API_KEY"   -d '{
  "model": "gpt-3.5-turbo",
  "messages": [
    {
      "role": "user",
      "content": ""
    },
    {
      "role": "assistant",
      "content": "Tell my a 10 jokes about software developer or system admins"
    }
  ],
  "temperature": 1,
  "max_tokens": 256,
  "top_p": 1,
  "frequency_penalty": 0,
  "presence_penalty": 0
}'
```

## How to get only the relevant content

### Ask the KI!

I execute a curl command and it returns this json data structure, how to get only the content?

```json
{
  "id": "chatcmpl-8VxjjQHEdphwpa7oKdfrNHU3IoX1p",
  "object": "chat.completion",
  "created": 1702628487,
  "model": "gpt-3.5-turbo-0613",
  "choices": [
    {
      "index": 0,
      "message": {
        "role": "assistant",
        "content": "1. Why do programmers prefer dark mode?\n   Because the light attracts too many bugs!\n\n2. Why do programmers always mix up Christmas and Halloween?\n   Because Oct 31 == Dec 25!\n\n3. Why was the computer cold?\n   It left its Windows open!\n\n4. Why do programmers prefer dogs over cats?\n   Because dogs have fetch, but cats have catch!\n\n5. Why don't programmers like nature?\n   It has too many bugs!\n\n6. Why did the software developer go broke?\n   Because he lost his domain in a bet!\n\n7. Why did the sysadmin bring a ladder to work?\n   To reach the cloud!\n\n8. How many software developers does it take to change a light bulb?\n   None, that's a hardware issue!\n\n9. Why did the sysadmin always carry a screwdriver?\n   Because he wanted to fix all the bugs physically!\n\n10. Why did the developer go broke after buying a yacht?\n    Because he spent all his money on floating-point operations!"
      },
      "finish_reason": "stop"
    }
  ],
  "usage": {
    "prompt_tokens": 23,
    "completion_tokens": 204,
    "total_tokens": 227
  },
  "system_fingerprint": null
}
```

#### Answer

You can use a tool like jq to parse the JSON data. jq is a lightweight and flexible command-line JSON processor. If it's not installed, you can install it using the package manager for your system.

Here's how you can extract the content using jq:

```sh
curl 'your-api-url' | jq -r '.choices[0].message.content'
```

This command will fetch the JSON from your API, pipe it to jq, and jq will extract the content field from the first object in the choices array. The -r option is used to output raw strings instead of JSON-encoded strings.

## Second Try

### Executing the command

```sh
curl https://api.openai.com/v1/chat/completions   -H "Content-Type: application/json"   -H "Authorization: Bearer $OPENAI_API_KEY"   -d '{
  "model": "gpt-3.5-turbo",
  "messages": [
    {
      "role": "assistant",
      "content": "Tell my a 10 jokes about software developer or system admins"
    }
  ],
  "max_tokens": 256
}' | jq -r '.choices[0].message.content'
```

### Output

```plain
1. Why did the software developer go broke?
   Because he didn't have enough cache!

2. Why don't software developers like nature?
   Because they prefer their own artificial intelligence!

3. Why did the system admin bring a ladder to work?
   To reach the cloud!

4. Why did the software developer wear glasses?
   Because he couldn't C# without them!

5. Why did the system admin cross the road?
   To troubleshoot a chicken's connectivity issues!

6. Why do software developers prefer dark mode?
   Because light mode is too bright for their coding adventures!

7. Why did the system admin bring a fire extinguisher to work?
   In case the servers caught fire from all the hot code!

8. Why did the software developer go to therapy?
   Because he could never get over his code dependencies!

9. Why do system admins make good firefighters?
   Because they excel at handling emergencies and putting out server fires!

10. Why did the software developer always carry a calculator?
    In case he needed to convert hexadecimal to ASCII to solve everyday problems!
```


## Be aware of content you send

```sh
CONTENT="Please audit my file /var/log/auth.log. Content of the file: $(cat /var/log/auth.log) Give a summary of only security related events."

curl https://api.openai.com/v1/chat/completions   -H "Content-Type: application/json"   -H "Authorization: Bearer $OPENAI_API_KEY"   -d "{
    \"model\": \"gpt-3.5-turbo\",
    \"messages\": [
      {
        \"role\": \"assistant\",
        \"content\": \" $(echo $CONTENT | jq -sRr . ) \"
      }
    ],
    \"max_tokens\": 2560
  }" | jq -r '.choices[0].message.content'
```

```plain
Summary of security-related events in /var/log/auth.log:

- Dec 15 09:35:58: Authentication failure for user "hdev" during sudo command execution.
- Dec 15 09:36:03: Conversation failed during authentication for user "hdev" with sudo command.
- Dec 15 09:36:03: Authentication could not identify the password for user "hdev" during sudo command execution.
- Dec 15 09:36:03: User "hdev" had 1 incorrect password attempt during sudo command execution.
```

### Nice but was that a good idea?

You just send your auth.log to a third party. This is not a good idea. You should be aware of what you send to the KI.

```
cat /var/log/auth.log
Dec 15 09:19:35 My-Notebook login[392]: pam_unix(login:session): session opened for user my-user-name(uid=1000) by (uid=0)
Dec 15 09:19:35 My-Notebook systemd-logind[183]: New session 1 of user my-user-name.
Dec 15 09:19:35 My-Notebook systemd: pam_unix(systemd-user:session): session opened for user my-user-name(uid=1000) by (uid=0)
Dec 15 09:35:58 My-Notebook sudo: pam_unix(sudo-i:auth): authentication failure; logname= uid=1000 euid=0 tty=/dev/pts/0 ruser=my-user-name rhost=  user=my-user-name
Dec 15 09:36:03 My-Notebook sudo: pam_unix(sudo-i:auth): conversation failed
Dec 15 09:36:03 My-Notebook sudo: pam_unix(sudo-i:auth): auth could not identify password for [my-user-name]
Dec 15 09:36:03 My-Notebook sudo:     my-user-name : 1 incorrect password attempt ; TTY=pts/0 ; PWD=/root ; USER=root ; COMMAND=/bin/bash
```
