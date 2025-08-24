"use client";
import { useState, useEffect, Dispatch, SetStateAction } from "react";
import { ITask, ITaskComment, IUser, UserRights } from "@/types";
import { Button } from "@/components/ui/button";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectValue,
  SelectTrigger,
} from "@/components/ui/select";
import { Avatar } from "@/components/ui/avatar";
import { Label } from "@/components/ui/label";
import { Dialog, DialogContent } from "@/components/ui/dialog";
import { X, Send } from "lucide-react";
import { useParams, useSearchParams } from "next/navigation";
import { cn } from "@/lib/utils";
import { Calendar as CalendarIcon } from "lucide-react";
import { Calendar } from "@/components/ui/calendar";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import dayjs from "dayjs";
import Editor from "@/components/editor";
import {
  HubConnectionBuilder,
  HubConnection,
  HubConnectionState,
} from "@microsoft/signalr";
import { Input } from "@/components/ui/input";
import { ScrollArea } from "../ui/scroll-area";
import { DeleteTask, GetStagesByProjectId, GetTask, GetUsersByProjectId, UpdateTaskDescription, UpdateTaskFinishDate, UpdateTaskName, UpdateTaskProgress, UpdateTaskResponsibleUser, UpdateTaskStage, UpdateTaskStartDate } from "@/requests";

interface ITaskModalProps {
  onClose: () => void;
  setFetch: Dispatch<SetStateAction<boolean>>;
  rights: UserRights | undefined;
}

interface IStageDto {
  id: string;
  name: string;
}

const TaskModal = ({ onClose, setFetch, rights }: ITaskModalProps) => {
  const params = useParams<{ id: string }>();
  const [isActive] = useState<boolean>(true);
  const searchParams = useSearchParams();
  const [currentTask, setTask] = useState<ITask | null>(null);
  const [responsibleUser, setResponsibleUser] = useState<IUser>({
    username: "Все",
  } as IUser);
  const [users, setUsers] = useState<IUser[]>([]);
  const [statusList, setStatusList] = useState<IStageDto[]>([]);
  const [status, setStatus] = useState<IStageDto>();
  const [progress, setProgress] = useState<string>("");
  const [name, setName] = useState<string>("");
  const [desc, setDesc] = useState<string>("");
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const [finishDate, setFinishDate] = useState<string>(
    dayjs().format("YYYY-MM-DD")
  );
  const [startDate, setStartDate] = useState<string>(
    dayjs().format("YYYY-MM-DD")
  );
  const [isModified, setIsModified] = useState<boolean>(false);
  const [message, setMessage] = useState<string>("");
  const [currentUserId, setCurrentUserId] = useState<string | null>(null);
  const [conn, setConnection] = useState<HubConnection | null>(null);
  const [taskComments, setTaskComments] = useState<ITaskComment[]>([]);

  useEffect(() => {
    getTaskInfo();
    getUserId();
  }, []);

  useEffect(() => {
    if (!currentTask) return;
    if (desc !== currentTask.description || name !== currentTask.name)
      setIsModified(true);
    else setIsModified(false);
  }, [desc, name]);

  useEffect(() => {
    if (!currentTask) return;
    if (currentTask.responsibleUserId === null)
      setResponsibleUser((prev) => ({
        ...prev,
        username: "Все",
      }));
    setName(currentTask.name);
    setDesc(currentTask.description);
    setProgress(currentTask.progress.toString());
    setStartDate(dayjs(currentTask.startDate).format("YYYY-MM-DD"));
    setFinishDate(dayjs(currentTask.finishDate).format("YYYY-MM-DD"));
  }, [currentTask]);

  useEffect(() => {
    if (currentTask && statusList !== undefined) {
      const currentStatus = statusList.find(
        (st) => st.id === currentTask.stageId
      );
      setStatus(currentStatus);
    }
  }, [statusList]);

  const changeProgress = async (value: string) => {
    const taskId = searchParams.get("task");
    if (!taskId) return;
    const response = await UpdateTaskProgress(taskId, value);
    setProgress(value);
  };

  const getUserId = () => {
    const userId = localStorage.getItem("userId");
    setCurrentUserId(userId);
  };

  const handleSaveStartDate = async (date: string) => {
    const taskId = searchParams.get("task");
    if (!taskId) return;
    const response = await UpdateTaskStartDate(taskId, date);
  };

  const handleSaveFinishDate = async (date: string) => {
    const taskId = searchParams.get("task");
    if (!taskId) return;
    const response = await UpdateTaskFinishDate(taskId, date);
  };

  const handleSaveChanges = async () => {
    const taskId = searchParams.get("task");
    if (!taskId) return;
    if (currentTask?.name !== name) saveName(taskId);
    if (currentTask?.description !== desc) saveDescription(taskId);
    setIsModified(false);
  };

  const handleChangeResponsibleUser = async (value: string) => {
    const taskId = searchParams.get("task");
    if (!taskId) return;
    if (value === "Все") {
      const response = await UpdateTaskResponsibleUser(taskId, "");
      setResponsibleUser({} as IUser);
    } else {
      const newResponsibleUser = users.find((us) => us.id === value);
      const newUsersArr = users.filter(
        (user) => user.id !== newResponsibleUser!.id
      );
      setUsers([...newUsersArr, responsibleUser]);
      setResponsibleUser(newResponsibleUser!);
      const response = await UpdateTaskResponsibleUser(taskId, value);
    }
  };

  const saveName = async (taskId: string) => {
    const response = await UpdateTaskName(taskId, name);
    return response;
  };

  const saveDescription = async (taskId: string) => {
    const response = await UpdateTaskDescription(taskId, desc);
    return response;
  };

  const getTaskInfo = async () => {
    const taskId = searchParams.get("task");
    if (!taskId) return;

    const response = await GetTask(taskId);
    const data = await response.json();

    getStagesInfo(data.task);
    setTask(data.task);
    setTaskComments(data.comments);

    const usersArr: IUser[] = await getUsers(params.id);
    const usersWithoutResponsible = usersArr.filter(
      (us) => us.id !== data.task.responsibleUserId
    );

    setUsers(usersWithoutResponsible);
    const user = usersArr.find((us) => us.id === data.task.responsibleUserId);
    if (user) setResponsibleUser(user!);
    else setResponsibleUser({ username: "Все" } as IUser);
  };

  const handleChangeStage = async (stageName: string | undefined) => {
    const taskId = searchParams.get("task");
    if (!taskId || stageName === undefined) return;
    const stage = statusList.find((st) => st.name === stageName)!;
    const response = await UpdateTaskStage(taskId, stage.id);
    setStatus(stage);
  };

  const getStagesInfo = async (task: ITask) => {
    const response = await GetStagesByProjectId(task.projectId);
    const data = await response.json();
    setStatusList(data);
  };

  const handleDelete = async () => {
    if (!currentTask) return;
    const response = await DeleteTask(currentTask.id);
    setFetch(true);
    onClose();
  };

  const getUsers = async (id: string) => {
    const response = await GetUsersByProjectId(id);
    const data = await response.json();
    return data;
  };

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
    if (isActive) start();
  }, [isActive]);

  try {
    if (conn) {
      const taskId = searchParams.get("task");
      if (!taskId) return;
      conn.on(`ReceiveTaskComment${taskId}`, (comment) => {
        setTaskComments([...taskComments, comment]);
      });
    }
  } catch (exception) {
    console.log(exception);
  }

  const sendMessage = async () => {
    if (!conn) return;
    const taskId = searchParams.get("task");
    if (!taskId || !currentUserId) return;
    if (conn.state === HubConnectionState.Connected) {
      conn.invoke("SendTaskComment", currentUserId, message, taskId);
      setMessage("");
    } else {
      console.log("sendMsg: " + conn.state);
    }
  };

  return (
    <div
      className="bg-white dark:bg-[#111] w-[140vh] h-[80vh] grid grid-cols-[3fr_1fr] p-[0.4rem]">
      <div
        className="border-r dark:border-gray-100 h-full flex flex-col"
        style={rights && (rights & UserRights.CanModifyTask) ? {} : { pointerEvents: "none" }}
      >
        <div className="flex flex-col justify-evenly">
          <div className="flex flex-row items-center w-[80%] mx-[3rem] my-[1rem]">
            <Label className="text-inherit text-[2rem] grow-[5]">
              Название
            </Label>
            <Input
              className="text-red text-2xl grow-[5] text-center rounded-none border-0 focus-visible:outline-none focus-visible:ring-0 focus-visible:ring-offset-0"
              onChange={(e) => setName(e.target.value)}
              value={name}
              placeholder={name}
            />
          </div>
          <div className="w-[80%] mx-[3rem] my-[1rem]">
            <Label className="text-inherit text-[2rem] grow-[5]">
              Описание
            </Label>
            <Editor
              desc={desc}
              setDesc={setDesc}
              styles="dark:border-black/40"
            />
          </div>
          <div className="flex flex-row">
            <div className="w-[40%] py-[0.3rem] my-[1rem] mx-[3rem] flex flex-row items-center justify-between">
              <div className="flex flex-col w-full">
                <Label
                  id="responsibleinput"
                  className="text-black mr-[1rem] mb-[0.2rem]"
                >
                  Исполнитель
                </Label>
                <Select
                  value={responsibleUser ? responsibleUser.id : "Все"}
                  onValueChange={(value) => handleChangeResponsibleUser(value)}
                >
                  <SelectTrigger className="dark:bg-[#111] dark:border-black/20">
                    <SelectValue placeholder="Выберите"></SelectValue>
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem key={"all"} value={"Все"}>
                      <div>Все</div>
                    </SelectItem>
                    {responsibleUser.firstName !== undefined ? (
                      <SelectItem
                        key={responsibleUser.id}
                        value={responsibleUser.id}
                      >
                        <div className="flex flex-row items-center gap-4">
                          <Avatar
                            className="bg-[#4198FF] text-white w-[2.5vh] h-[2.5vh] text-[0.6rem] m-[0.1rem] ml-[0.4rem] flex justify-center items-center"
                            // src="/static/images/avatar/1.jpg"
                          >
                            {responsibleUser?.firstName?.slice(0, 1)}
                            {responsibleUser?.lastName?.slice(0, 1)}
                          </Avatar>
                          <div>
                            {responsibleUser.lastName +
                              " " +
                              responsibleUser.firstName}
                          </div>
                        </div>
                      </SelectItem>
                    ) : (
                      <></>
                    )}
                    {users.map((user) => (
                      <SelectItem key={user.id} value={user.id}>
                        <div className="flex flex-row items-center gap-2">
                          {user.username !== "Все" ? (
                            <Avatar
                              className="bg-[#4198FF] text-white w-[2.5vh] h-[2.5vh] text-[0.6rem] m-[0.1rem] ml-[0.4rem] flex justify-center items-center"
                              // src="/static/images/avatar/1.jpg"
                            >
                              {user?.firstName?.slice(0, 1)}
                              {user?.lastName?.slice(0, 1)}
                            </Avatar>
                          ) : (
                            <div></div>
                          )}
                          <div>
                            {user.username !== "Все"
                              ? user.lastName + " " + user.firstName
                              : "Все"}
                          </div>
                        </div>
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </div>
            </div>
            <div className="w-[40%] py-[0.3rem] mx-[3rem] my-[1rem] flex flex-row items-center justify-between">
              <div className="flex flex-col w-full">
                <Label
                  id="statusinput"
                  className="text-black mr-[1rem] mb-[0.2rem]"
                >
                  Статус
                </Label>
                <Select
                  value={status?.name || ""}
                  onValueChange={(value) => handleChangeStage(value)}
                >
                  <SelectTrigger
                    className="SelectTrigger dark:bg-[#111] dark:border-black/20"
                    aria-label="Food"
                  >
                    <SelectValue placeholder="Выберите стадию" />
                  </SelectTrigger>
                  <SelectContent>
                    {statusList &&
                      statusList?.map((status) => (
                        <SelectItem
                          key={status.id}
                          value={status.name.toString()}
                        >
                          {status.name}
                        </SelectItem>
                      ))}
                  </SelectContent>
                </Select>
              </div>
            </div>
          </div>
          <div className="flex flex-col ml-12 mr-12 w-2/5">
            <Label className="mb-[0.2rem]">Прогресс</Label>
            <Select
              value={progress}
              onValueChange={(value) => {
                changeProgress(value);
              }}
            >
              <SelectTrigger
                className="SelectTrigger dark:bg-[#111] dark:border-black/20"
                aria-label="Food"
              >
                <SelectValue placeholder="Прогресс" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value={"0"}>0</SelectItem>
                <SelectItem value={"25"}>25</SelectItem>
                <SelectItem value={"50"}>50</SelectItem>
                <SelectItem value={"75"}>75</SelectItem>
                <SelectItem value={"100"}>100</SelectItem>
              </SelectContent>
            </Select>
          </div>
          <div className="flex flex-row">
            <div className="flex flex-col mx-[3rem] my-[1rem] py-[0.6rem] w-[40%]">
              <Label className="mb-[0.2rem]">Начало</Label>
              <Popover>
                <PopoverTrigger
                  asChild
                  className="dark:bg-[#111] dark:border-black/20 dark:hover:bg-black/20"
                >
                  <Button
                    variant={"outline"}
                    className={cn(
                      "w-[240px] justify-start text-left font-normal w-full",
                      !startDate && "text-muted-foreground"
                    )}
                  >
                    <CalendarIcon className="mr-2 h-4 w-4" />
                    {startDate ? (
                      dayjs(startDate).format("YYYY-MM-DD")
                    ) : (
                      <span>Выберите начало</span>
                    )}
                  </Button>
                </PopoverTrigger>
                <PopoverContent className="w-auto p-0" align="start">
                  <Calendar
                    mode="single"
                    selected={new Date(startDate)}
                    onSelect={(value) => {
                      setStartDate(dayjs(value).format("YYYY-MM-DD"));
                      handleSaveStartDate(dayjs(value).format("YYYY-MM-DD"));
                    }}
                  />
                </PopoverContent>
              </Popover>
            </div>
            <div className="flex flex-col mx-[3rem] my-[1rem] py-[0.6rem] w-[40%]">
              <Label className="mb-[0.2rem]">Конец</Label>
              <Popover>
                <PopoverTrigger
                  asChild
                  className="dark:bg-[#111] dark:border-black/20  dark:hover:bg-black/20"
                >
                  <Button
                    variant={"outline"}
                    className={cn(
                      "w-[240px] justify-start text-left font-normal w-full",
                      !finishDate && "text-muted-foreground"
                    )}
                  >
                    <CalendarIcon className="mr-2 h-4 w-4" />
                    {startDate ? (
                      dayjs(finishDate).format("YYYY-MM-DD")
                    ) : (
                      <span>Выберите конец</span>
                    )}
                  </Button>
                </PopoverTrigger>
                <PopoverContent className="w-auto p-0" align="start">
                  <Calendar
                    mode="single"
                    selected={new Date(finishDate)}
                    onSelect={(value) => {
                      setFinishDate(dayjs(value).format("YYYY-MM-DD"));
                      handleSaveFinishDate(dayjs(value).format("YYYY-MM-DD"));
                    }}
                  />
                </PopoverContent>
              </Popover>
            </div>
          </div>
          <div className="flex justify-between mr-[4.8rem] ml-[3rem]">
            <Button
              className={
                isModified
                  ? "border border-green-500 bg-white text-green-500 hover:bg-green-500 hover:text-white ease-in-out duration-300 dark:bg-green-500 dark:text-white dark:hover:bg-green-600"
                  : "invisible"
              }
              onClick={() => handleSaveChanges()}
            >
              Применить
            </Button>
            <Button
              variant={"destructive"}
              className={rights && (rights & UserRights.CanDeleteTask) ? "" : "invisible"}
              onClick={() => setIsModalOpen(true)}
            >
              Удалить
            </Button>
          </div>
        </div>
      </div>
      <Dialog open={isModalOpen} onOpenChange={setIsModalOpen}>
        <DialogContent className="sm:max-w-[425px]">
          <div className="flex flex-col justify-center bg-white w-full gap-2 rounded-[8px] p-[10px]">
            <Label className="text-black self-center">
              Вы действительно хотите удалить задачу?
            </Label>
            <div className="text-black self-center">
              <Button
                className="text-white bg-green-600 hover:bg-green-700 mr-[0.4rem]"
                onClick={() => handleDelete()}
              >
                Да
              </Button>
              <Button
                className="text-white bg-red-600 hover:bg-red-900"
                onClick={() => setIsModalOpen(false)}
              >
                Нет
              </Button>
            </div>
          </div>
        </DialogContent>
      </Dialog>
      <div>
        <div className="h-full flex flex-col justify-between">
          <div className="flex flex-row p-[0.6rem] justify-between">
            <Label className="text-[1.2rem] ml-[2rem]">{`Комментарии`}</Label>
            <X
              onClick={() => {
                onClose();
                setFetch(true);
              }}
              className="text-[1.6rem] p-[0.1rem] self-center hover:rounded-[12px] ease-in-out duration-200"
            />
          </div>
          <ScrollArea className="max-h-[58vh] min-h-[48vh] overflow-auto flex flex-col px-[1rem] grow">
            {taskComments.map((message, index) =>
              currentUserId && message.userId === currentUserId ? (
                <div key={index} className="flex flex-col">
                  <div className="self-end">
                    <Label className="text-center text-[0.7rem] text-[#87888C]">
                      {message
                        ? dayjs(message.time).format("DD.MM.YY HH:mm")
                        : ""}
                    </Label>
                  </div>
                  <div className="flex justify-end my-[0.15rem]">
                    <div className="bg-[#3288F0] px-[0.8rem] py-[0.2rem] rounded-[0.6rem]">
                      <div className="flex justify-between items-baseline">
                        <Label className="text-white dark:text-white">
                          {message.content}
                        </Label>
                      </div>
                    </div>
                  </div>
                </div>
              ) : (
                <div key={index}>
                  <div>
                    <Label className="text-center text-[0.7rem] text-[#87888C]">
                      {message
                        ? dayjs(message.time).format("DD.MM.YY HH:mm")
                        : ""}
                    </Label>
                  </div>
                  <div className="flex justify-start my-[0.15rem]">
                    <div className="bg-[#ECF0F3] px-[0.8rem] py-[0.2rem] rounded-[0.6rem]">
                      <Label className="text-[#87888C] text-[0.7rem]">
                        {message.username}
                      </Label>
                      <div className="flex justify-between items-baseline">
                        <Label>{message.content}</Label>
                      </div>
                    </div>
                  </div>
                </div>
              )
            )}
          </ScrollArea>
          <div className="flex justify-center">
            <div className="px-[0.8rem] py-[0.4rem] rounded-[0.6rem] flex justify-between items-center w-[80%] gap-2 mt-[0.5rem] mb-[1vh]">
              <Input
                className="flex-1 dark:text-white dark:bg-[#111] dark:border-black/20"
                placeholder="Написать сообщение..."
                value={message}
                onChange={(e) => setMessage(e.target.value)}
              />
              <Send
                className="text-[0.9rem] text-[rgba(102,153,255,0.6)]"
                onClick={() => sendMessage()}
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default TaskModal;
