"use client";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import { Dialog, DialogContent } from "@/components/ui/dialog";
import { useState, useEffect } from "react";
import { useParams, useRouter } from "next/navigation";
import { IProject, IUser } from "@/types";
import { CalendarDays, CalendarIcon, Circle } from "lucide-react";
import Editor from "@/components/editor";
import dayjs from "dayjs";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { cn } from "@/lib/utils";
import {
  HubConnectionBuilder,
  HubConnection,
  LogLevel,
  HubConnectionState,
} from "@microsoft/signalr";

export default function Page() {
  const { id } = useParams<{ id: string }>();
  const router = useRouter();
  const [projectSettings, setProjectSettings] = useState<IProject>(
    {} as IProject
  );
  const [users, setUsers] = useState<IUser[]>([]);
  const [conn, setConnection] = useState<HubConnection | null>(null);
  const [isProjectModalOpen, setIsProjectModalOpen] = useState<boolean>(false);
  const [name, setName] = useState<string>("");
  const [desc, setDesc] = useState<string>("");
  const [isModified, setIsModified] = useState<boolean>(false);

  useEffect(() => {
    async function fetchInfo() {
      const response = await fetch(
        `http://localhost:5000/api/project/${id}/settings`,
        {
          method: "GET",
          headers: { "Content-Type": "application/json" },
          credentials: "include",
        }
      );
      if (response.ok) {
        const data = await response.json();
        setProjectSettings(data);
        setDesc(data.description);
        setName(data.name);
        setProjectSettings((prev) => ({
          ...prev,
          users: [...data.users, data.owner],
        }));
      }
    }
    fetchInfo();
  }, []);

  useEffect(() => {
    if (name !== projectSettings.name || desc !== projectSettings.description)
      setIsModified(true);
    else setIsModified(false);
  }, [desc, name]);

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
    start();
  }, []);

  const sendMessageDeleteProject = async (projectId: string) => {
    if (!conn) return;
    if (conn.state === HubConnectionState.Connected) {
      conn.invoke("FetchDeleteProject", projectId);
    } else {
      console.log("sendMsg: " + conn.state);
    }
  };

  const sendMessageChangeProjectName = async (projectId: string) => {
    if (!conn) return;
    if (conn.state === HubConnectionState.Connected) {
      conn.invoke("FetchChangeProjectName", projectId);
    } else {
      console.log("sendMsg: " + conn.state);
    }
  };

  const saveProjectChanges = () => {
    if (desc !== projectSettings.description) handleChangeProjectDescription();
    if (name !== projectSettings.name) handleChangeProjectName();
    setIsModified(false);
  };

  const handleDeleteProject = async () => {
    const response = await fetch(`http://localhost:5000/api/project/${id}`, {
      method: "DELETE",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    });
    if (response.ok === true) {
      sendMessageDeleteProject(id);
      router.push("/main");
    }
  };

  const handleChangeProjectName = async () => {
    const response = await fetch(
      `http://localhost:5000/api/project/${projectSettings.id}/name?name=${name}`,
      {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      }
    );
    sendMessageChangeProjectName(projectSettings.id);
    setProjectSettings((prev) => ({
      ...prev,
      name: name,
    }));
  };

  const handleChangeProjectDescription = async () => {
    const response = await fetch(
      `http://localhost:5000/api/project/${projectSettings.id}/desc?description=${desc}`,
      {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      }
    );
    setProjectSettings((prev) => ({
      ...prev,
      description: desc,
    }));
  };

  return (
    <>
      <div className="flex flex-row justify-evenly w-full">
        <div style={{ display: "flex", flexDirection: "column" }}>
          <Label className="ml-1 mb-2 text-[1rem]">Имя проекта</Label>
          <Input
            className="focus-visible:outline-none focus-visible:ring-0 focus-visible:ring-slate-950 focus-visible:ring-offset-0 dark:border-gray-50"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />
          <div className="w-full mt-2 mb-2">
            <Label className="ml-1 text-[1rem]">Описание проекта</Label>
            <Editor desc={desc} setDesc={setDesc} />
          </div>
          <div className="flex flex-row gap-8">
            <Button
              onClick={() => setIsProjectModalOpen(true)}
              variant={"destructive"}
            >
              Удалить проект
            </Button>
            <Button
              className={
                isModified
                  ? "border border-green-500 bg-white text-green-500 hover:bg-green-500 hover:text-white ease-in-out duration-300"
                  : "invisible"
              }
              onClick={() => saveProjectChanges()}
            >
              Сохранить изменения
            </Button>
          </div>
        </div>
        <div className="flex flex-col">
          <div className="flex flex-row items-center gap-2">
            <CalendarDays size={18} />
            <Label className="text-[1rem]">Сроки проекта</Label>
          </div>
          <div className="rounded-lg bg-gray-50 p-2 dark:bg-black/40">
            <div className="flex flex-row items-center justify-between">
              <Label>Начало</Label>
              <Popover>
                <PopoverTrigger asChild>
                  <Button
                    variant={"ghost"}
                    className={cn(
                      "justify-start text-left font-normal dark:bg-[#111] dark:border-black/30 dark:hover:bg-black/20 hover:bg-transparent hover:cursor-default dark:bg-transparent dark:hover:bg-transparent",
                      !projectSettings.startDate && "text-muted-foreground"
                    )}
                  >
                    <CalendarIcon className="mr-2 h-4 w-4" />
                    {projectSettings.startDate ? (
                      dayjs(projectSettings.startDate).format("DD-MM-YYYY")
                    ) : (
                      <span>Выберите дату</span>
                    )}
                  </Button>
                </PopoverTrigger>
              </Popover>
              <Circle size={12} color="#009900" className="fill-green-600" />
            </div>
            <div className="flex flex-row items-center justify-between">
              <Label>Конец</Label>
              <Popover>
                <PopoverTrigger asChild>
                  <Button
                    variant={"ghost"}
                    className={cn(
                      "justify-start text-left font-normal dark:bg-[#111] dark:border-black/30 dark:hover:bg-black/20 hover:bg-transparent hover:cursor-default dark:bg-transparent dark:hover:bg-transparent",
                      !projectSettings.finishDate && "text-muted-foreground"
                    )}
                  >
                    <CalendarIcon className="mr-2 h-4 w-4" />
                    {projectSettings.startDate ? (
                      dayjs(projectSettings.finishDate).format("DD-MM-YYYY")
                    ) : (
                      <span>Выберите дату</span>
                    )}
                  </Button>
                </PopoverTrigger>
              </Popover>
              <Circle size={12} color="red" className="fill-red-600" />
            </div>
            <div className="flex flex-row items-center justify-end">
              <Label>Сегодня</Label>
              <Popover>
                <PopoverTrigger asChild>
                  <Button
                    variant={"ghost"}
                    className={cn(
                      "justify-start text-left font-normal dark:bg-[#111] dark:border-black/30 dark:hover:bg-black/20 hover:bg-transparent hover:cursor-default dark:bg-transparent dark:hover:bg-transparent",
                      !projectSettings.finishDate && "text-muted-foreground"
                    )}
                  >
                    <CalendarIcon className="mr-2 h-4 w-4" />
                    {projectSettings.startDate ? (
                      dayjs().format("DD-MM-YYYY")
                    ) : (
                      <span>Выберите дату</span>
                    )}
                  </Button>
                </PopoverTrigger>
              </Popover>
              <Circle size={12} color="orange" className="fill-orange-400" />
            </div>
          </div>
        </div>
      </div>
      <Dialog open={isProjectModalOpen}>
        <DialogContent>
          <div
            style={{
              display: "flex",
              flexDirection: "column",
              justifyContent: "center",
              backgroundColor: "white",
              width: "100%",
              borderRadius: "8px",
              padding: "10px",
              gap: 8,
            }}
          >
            <Label style={{ color: "black", alignSelf: "center" }}>
              Вы действительно хотите удалить проект?
            </Label>
            <div style={{ display: "flex", alignSelf: "center" }}>
              <Button
                onClick={() => handleDeleteProject()}
                style={{
                  color: "white",
                  backgroundColor: "green",
                  marginRight: "0.4rem",
                }}
              >
                Да
              </Button>
              <Button
                onClick={() => setIsProjectModalOpen(false)}
                style={{ color: "white", backgroundColor: "red" }}
              >
                Нет
              </Button>
            </div>
          </div>
        </DialogContent>
      </Dialog>
    </>
  );
}
