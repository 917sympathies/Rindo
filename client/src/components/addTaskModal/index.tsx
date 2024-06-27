"use client";
import { useState, useEffect, SetStateAction, Dispatch } from "react";
import { Button } from "../ui/button";
import { Input } from "../ui/input";
import { Label } from "../ui/label";
import {
  Select,
  SelectItem,
  SelectContent,
  SelectTrigger,
  SelectValue,
} from "../ui/select";
import { cn } from "@/lib/utils";
import { Calendar as CalendarIcon } from "lucide-react";
import { Calendar } from "@/components/ui/calendar";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { Avatar } from "../ui/avatar";
import { X } from "lucide-react";
import React from "react";
import { IProject, ITask, IUser } from "@/types";
import dayjs from "dayjs";
import { useParams } from "next/navigation";
import { IUserInfo } from "@/types";
import Editor from "../editor";
import {
  HubConnectionBuilder,
  HubConnection,
  LogLevel,
  HubConnectionState
} from "@microsoft/signalr";

interface IAddTaskModalProps {
  stageId: string | undefined;
  setFetch: Dispatch<SetStateAction<boolean>>;
  onClose: () => void;
}

const AddTaskModal = ({
  onClose,
  stageId,
  setFetch,
}:
IAddTaskModalProps) => {
  const { id } = useParams<{ id: string }>();
  const [task, setTask] = useState<ITask>({} as ITask);
  const [responsibleUser, setResponsibleUser] = useState("");
  const [desc, setDesc] = useState<string>("");
  const [startDate, setStart] = useState(dayjs().format("YYYY-MM-DD"));
  const [finishDate, setFinish] = useState(dayjs().format("YYYY-MM-DD"));
  const [users, setUsers] = useState<IUserInfo[] | null>(null);
  const [errorMessage, setErrorMessage] = useState<string>("");
  const [conn, setConnection] = useState<HubConnection | null>(null);

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


  const sendSignal = async () => {
    if(!conn) return;
    if (conn.state === HubConnectionState.Connected) {
      conn.invoke("SendTaskAdd", id);
    } else {
      console.log("sendMsg: " + conn.state);
    }
  };


  useEffect(() => {
    setTask((prevState) => ({
      ...prevState,
      description: desc,
    }));
  }, [desc]);

  useEffect(() => {
    getUsers(id);
  }, [id]);

  const getUsers = async (id: string) => {
    const response = await fetch(
      `http://localhost:5000/api/user?projectId=${id}`,
      {
        method: "GET",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      }
    );
    const data = await response.json();
    setUsers(data);
  };

  const handleChangeResponsibleUser = (value: string) => {
    if (value === "все") setResponsibleUser("");
    else setResponsibleUser(value);
  };

  const handleAddTask = async (
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    if (new Date(startDate) > new Date(finishDate)) {
      setErrorMessage("Вы выбрали некорректные даты!");
      return;
    }
    const taskDto = {
      name: task.name,
      description: task.description,
      projectId: id,
      stageId: stageId,
      comments: null,
      responsibleUserId: responsibleUser,
      startDate: startDate,
      finishDate: finishDate,
    } as ITask;
    const response = await fetch("http://localhost:5000/api/task", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
      body: JSON.stringify(taskDto),
    });
    sendSignal();
    setFetch(true);
    onClose();
    console.log(response);
  };

  return (
    <>
      <div className="max-w-[100vw] grid grid-rows-[1fr_15fr_1fr] h-full px-[3rem] py-[1rem]">
        <div className="bg-inherit flex justify-between items-center">
          <Label className="whitespace-nowrap overflow-hidden text-ellipsis text-[1.8rem] font-medium">
            Задача
          </Label>
          <X
            size={28}
            className="hover:bg-gray-50 transition ease-in-out duration-300 rounded-full p-[0.4vh] cursor-pointer"
            onClick={onClose}
          />
        </div>
        <div>
          <div className="mb-[.5rem]">
            <Label className="text-[1.4rem] font-medium whitespace-nowrap overflow-hidden text-ellipsis">
              Название задачи
            </Label>
            <Input
              onChange={(event) => {
                setTask((prevState) => ({
                  ...prevState,
                  name: event.target.value,
                }));
              }}
              placeholder="Без названия"
              className="w-full dark:border-black/30 text-[1.1rem] font-normal"
            />
          </div>
          <div className="flex flex-col max-w-[calc(50vw - 6rem)]">
            <Label className="text-black text-[1.4rem] font-medium whitespace-nowrap overflow-hidden text-ellipsis mt-[1rem] mb-[0.5rem]">
              Описание задачи
            </Label>
            <div className="relative overflow-x-hidden overwlow-y-auto max-w-full">
              <Editor
                desc={desc}
                setDesc={setDesc}
                styles="dark:border-black/40"
              />
            </div>
          </div>
          <div>
            <Label id="responsibleinput" className="mt-[10px] text-[1rem]">
              Ответственный
            </Label>
            <Select
              value={responsibleUser}
              onValueChange={(value) => handleChangeResponsibleUser(value)}
            >
              <SelectTrigger
                className="SelectTrigger dark:bg-[#111] dark:border-black/20"
                aria-label="Food"
              >
                <SelectValue placeholder="Все" />
              </SelectTrigger>
              <SelectContent className="dark:bg-[#111]">
                <SelectItem
                  className="dark:hover:bg-black pl-[2.5rem]"
                  value={"все"}
                >
                  Все
                </SelectItem>
                {users &&
                  users.map((user) => (
                    <SelectItem key={user.username} value={user.id}>
                      <div className="flex flex-row items-center gap-2">
                        <Avatar className="bg-[#4198FF] text-white w-[2.5vh] h-[2.5vh] text-[0.6rem] m-[0.1rem] ml-[0.4rem] flex justify-center items-center"
                          // src="/static/images/avatar/1.jpg"
                        >
                          {user?.firstName?.slice(0, 1)}
                          {user?.lastName?.slice(0, 1)}
                        </Avatar>
                        <div>
                          {user.firstName +
                            " " +
                            user.lastName +
                            " (" +
                            user.username +
                            ")"}
                        </div>
                      </div>
                    </SelectItem>
                  ))}
              </SelectContent>
            </Select>
          </div>
          <div className="mt-[1rem] flex flex-col gap-2">
            <div className="flex flex-col">
              <Label className="mb-[0.2rem] text-[1rem]">Начало</Label>
              <Popover>
                <PopoverTrigger asChild>
                  <Button
                    variant={"outline"}
                    className={cn(
                      "w-[240px] justify-start text-left font-normal dark:bg-[#111] dark:border-black/30 dark:hover:bg-black/20",
                      !startDate && "text-muted-foreground"
                    )}
                  >
                    <CalendarIcon className="mr-2 h-4 w-4" />
                    {startDate ? (
                      dayjs(startDate).format("YYYY-MM-DD")
                    ) : (
                      <span>Pick a date</span>
                    )}
                  </Button>
                </PopoverTrigger>
                <PopoverContent className="w-auto p-0" align="start">
                  <Calendar
                    mode="single"
                    selected={new Date(startDate)}
                    onSelect={(value) =>
                      setStart(dayjs(value).format("YYYY-MM-DD"))
                    }
                    initialFocus
                  />
                </PopoverContent>
              </Popover>
            </div>
            <div className="flex flex-col">
              <Label className="mb-[0.2rem] text-[1rem]">Конец </Label>
              <Popover>
                <PopoverTrigger asChild>
                  <Button
                    variant={"outline"}
                    className={cn(
                      "w-[240px] justify-start text-left font-normal dark:bg-[#111] dark:border-black/30 dark:hover:bg-black/20",
                      !finishDate && "text-muted-foreground"
                    )}
                  >
                    <CalendarIcon className="mr-2 h-4 w-4" />
                    {finishDate ? (
                      dayjs(finishDate).format("YYYY-MM-DD")
                    ) : (
                      <span>Pick a date</span>
                    )}
                  </Button>
                </PopoverTrigger>
                <PopoverContent className="w-auto p-0" align="start">
                  <Calendar
                    mode="single"
                    selected={new Date(finishDate)}
                    onSelect={(value) =>
                      setFinish(dayjs(value).format("YYYY-MM-DD"))
                    }
                    initialFocus
                  />
                </PopoverContent>
              </Popover>
            </div>
          </div>
          <div>
            {errorMessage === "" ? (
              <div></div>
            ) : (
              <p className="mt-[2rem] text-[1.2rem] font-medium text-red-800">{errorMessage}</p>
            )}
          </div>
        </div>
        <div>
          <Button
            className="bg-sky-600 border border-sky-600 text-[1rem] text-white hover:bg-sky-700 ease-in-out"
            onClick={(e) => handleAddTask(e)}
          >
            Добавить задачу
          </Button>
        </div>
      </div>
    </>
  );
};

export default AddTaskModal;
