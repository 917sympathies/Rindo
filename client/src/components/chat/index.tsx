"use client";
import { useState, useEffect, Dispatch, SetStateAction } from "react";
import { X } from "lucide-react";
import { Label } from "../ui/label";
import { Input } from "../ui/input";
import { Send } from "lucide-react";
import { IChat, IUserInfo, IMessage, ICookieInfo } from "@/types";
import { jwtDecode } from "jwt-decode";
import { useCookies } from "react-cookie";
import {
  HubConnectionBuilder,
  HubConnection,
  LogLevel,
  HubConnectionState
} from "@microsoft/signalr";

interface ChatProps {
  chatId: string | undefined;
  projectName: string | undefined;
  isActive: boolean;
  setIsChatActive: Dispatch<SetStateAction<boolean>>
}

export default function Chat({
  isActive,
  setIsChatActive,
  chatId,
  projectName,
}: ChatProps) {
  const [message, setMessage] = useState<string>("");
  const [chatMessages, setChatMessages] = useState<IMessage[]>([])
  const [chat, setChat] = useState<IChat | null>(null);
  const [user, setUser] = useState<IUserInfo | null>(null);
  const [conn, setConnection] = useState<HubConnection | null>(null);
  const [cookies] = useCookies();

  useEffect(() => {
    const getChatInfo = async () => {
      const response = await fetch(`http://localhost:5000/api/chat/${chatId}`, {
        method: "GET",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      });
      const data = await response.json();
      console.log(data)
      setChat(data.value);
      setChatMessages(data.value.messages)
    };
    const getUserInfo = async () => {
      // const token = cookies["test-cookies"];
      // if (!token) return;
      // const decoded = jwtDecode(token) as ICookieInfo;
      const userId = localStorage.getItem("userId");
      const response = await fetch(
        `http://localhost:5000/api/user/${userId}`,
        {
          method: "GET",
          headers: { "Content-Type": "application/json" },
          credentials: "include",
        }
      );
      const data = await response.json();
      setUser(data);
    };
    getChatInfo();
    getUserInfo();
  }, [isActive]);

  useEffect(() => {
    async function start() {
      let connection = new HubConnectionBuilder()
        .withUrl(`http://localhost:5000/chat`)
        .build();
      setConnection(connection);
      if (connection.state === HubConnectionState.Disconnected) {
        await connection.start();
      } else {
        console.log("Already connected");
      }
    }
    if(isActive) start();
  }, [isActive, chatId]);

  try {
    if(conn){
    conn.on(`ReceiveProjectChat${chatId}`, (id, username, message) => {
      const str = {id: id, username: username, content: message} as IMessage;
      setChatMessages([...chatMessages, str])
    });
    }
  } catch (exception) {
    console.log(exception);
  }

  try {
    if(conn){
    conn.on(`HelloMsg`, (message) => {
      console.log(message);
    });
  }
  } catch (exception) {
    console.log(exception);
  }

  const sendMessage = async () => {
    if(!conn) return;
    if (conn.state === HubConnectionState.Connected) {
      conn.invoke("SendProjectChat", user?.id, message, chatId);
      setMessage("");
    } else {
      console.log("sendMsg: " + conn.state);
    }
  };

  return (
    <div className="flex flex-col justify-between w-full h-full mb-[2rem]">
      <div className="flex flex-row justify-between p-4">
        <Label className="text-[1.2rem] pl-[2rem]">
          {`Чат проекта: ${projectName}`}
        </Label>
        <X onClick={() => setIsChatActive(false)} className=""/>
      </div>
      <div>
        <div
          style={{
            maxHeight: "48vh",
            minHeight: "48vh",
            overflow: "auto",
            display: "flex",
            flexDirection: "column",
            padding: "0 1rem",
          }}
        >
          {chatMessages.length == 0 ? 
          <div className="flex self-center text-gray-400">
            Тут пока нет сообщений...
          </div> : <></>}
          {chatMessages?.map((message, index) =>
            user && message.username === user?.username ? (
              <div key={index}>
                <div>
                  <Label
                    style={{
                      textAlign: "center",
                      fontSize: ".8rem",
                      color: "#87888C",
                    }}
                  >
                    {/* {message ? moment(message.createdAt).format("DD.MM") : ""} */}
                  </Label>
                </div>
                <div
                  style={{
                    display: "flex",
                    justifyContent: "flex-end",
                    margin: ".15rem 0",
                  }}
                >
                  <div
                    style={{
                      backgroundColor: "#3288F0",
                      padding: ".2rem .8rem",
                      borderRadius: ".6rem",
                    }}
                  >
                    <div
                      style={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "baseline",
                      }}
                    >
                      <Label
                        style={{
                          fontSize: ".9rem",
                          fontWeight: "500",
                          color: "white",
                        }}
                      >
                        {message.content}
                      </Label>
                      <Label
                        style={{
                          fontWeight: "500",
                          fontSize: ".6rem",
                          color: "white",
                          marginLeft: ".5rem",
                        }}
                      >
                        {/* {moment(message.createdAt).format("HH:mm")} */}
                      </Label>
                    </div>
                  </div>
                </div>
              </div>
            ) : (
              <div key={index}>
                <div>
                  <Label
                    style={{
                      textAlign: "center",
                      fontSize: ".8rem",
                      color: "#87888C",
                    }}
                  >
                    {/* {message ? moment(message.createdAt).format("DD.MM") : ""} */}
                  </Label>
                </div>
                <div
                  style={{
                    display: "flex",
                    justifyContent: "flex-start",
                    margin: ".15rem 0",
                  }}
                >
                  <div
                    style={{
                      backgroundColor: "#ECF0F3",
                      padding: ".2rem .8rem",
                      borderRadius: ".6rem",
                    }}
                  >
                    <Label color={"#87888C"} style={{ fontSize: ".7rem", color: "#87888C" }}>
                      {message.username}
                    </Label>
                    <div
                      style={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "baseline",
                      }}
                    >
                      <Label
                        style={{
                          fontWeight: "500",
                          fontSize: ".9rem",
                          color: "black",
                        }}
                      >
                        {message.content}
                      </Label>
                      <Label
                        style={{
                          fontSize: ".6rem",
                          fontWeight: "500",
                          color: "black",
                          marginLeft: ".9rem",
                        }}
                      >
                        {/* {moment(message.createdAt).format("HH:mm")} */}
                      </Label>
                    </div>
                  </div>
                </div>
              </div>
            )
          )}
        </div>
      </div>
      <div className="flex justify-center">
        <div className="py-[.6rem] px-[.8rem] flex justify-between items-center mt-[.5rem] mr-0 mb-[1vh] ml-0 gap-7 w-[85%]">
          <Input
            className="flex-1 dark:text-white dark:bg-[#111] dark:border-black/20"
            placeholder="Написать сообщение..."
            value={message}
            onChange={(e) => setMessage(e.target.value)}
            onKeyDown={(e) => {
              if(e.key === 'Enter') sendMessage();
            }}
          />
          <Send className="text-[.9rem] text-sky-600 hover:text-sky-800 ease-in-out duration-300"
            onClick={() => sendMessage()}
          />
        </div>
      </div>
    </div>
  );
}
