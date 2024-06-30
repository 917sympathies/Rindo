"use client";
import styles from "./styles.module.css";
import { useState, useEffect, Dispatch, SetStateAction } from "react";
import { ITask, ITaskComment, IUser, IUserInfo, ICookieInfo, IUserRights } from "@/types";
import { Button } from "@/components/ui/button";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectValue,
  SelectTrigger,
} from "@/components/ui/select";
import { Avatar } from "@/components/ui/avatar";
import { ArrowDown } from "lucide-react";
import { Label } from "@/components/ui/label";
import {
  Dialog,
  DialogContent,
  DialogOverlay,
  DialogPortal,
} from "@/components/ui/dialog";
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
  LogLevel,
  HubConnectionState,
} from "@microsoft/signalr";
import { jwtDecode } from "jwt-decode";
import { useCookies } from "react-cookie";
import { Input } from "@/components/ui/input";

interface ITaskModalProps {
  onClose: () => void;
  setFetch: Dispatch<SetStateAction<boolean>>;
  rights:  IUserRights;
}

interface IStageDto {
  id: string;
  name: string;
}

const TaskModal = ({ onClose, setFetch, rights }: ITaskModalProps) => {
  const params = useParams<{id: string}>();
  const [isActive] = useState<boolean>(true);
  const searchParams = useSearchParams();
  const [cookies] = useCookies();
  const [currentTask, setTask] = useState<ITask | null>(null);
  const [responsibleUser, setResponsibleUser] = useState<IUser>({
    username: "Все",
  } as IUser);
  const [users, setUsers]= useState<IUser[]>([]);
  const [statusList, setStatusList] = useState<IStageDto[]>([]);
  const [status, setStatus] = useState<IStageDto>();
  const [progress, setProgress] = useState<string>("");
  const [name, setName] = useState<string>("");
  const [desc, setDesc] = useState<string>("");
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const [finishDate, setFinishDate] = useState<string>(dayjs().format("YYYY-MM-DD"));
  const [startDate, setStartDate] = useState<string>(dayjs().format("YYYY-MM-DD"));
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
    if (desc !== currentTask.description || name !== currentTask.name) setIsModified(true);
    else setIsModified(false);
  }, [desc, name]);

  useEffect(() => {
    if (!currentTask) return;
    if (currentTask.responsibleUserId === null)
      setResponsibleUser((prev) => ({
        ...prev,
        username: "Все",
      }));
    else {
      getUserInfo(currentTask.responsibleUserId);
    }
    setName(currentTask.name);
    setDesc(currentTask.description);
    setProgress(currentTask.progress.toString());
    //if (currentTask.comments) setTaskComments(currentTask.comments);
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
    const response = await fetch(
      `http://localhost:5000/api/task/${taskId}/progress?number=${value}`,
      {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      }
    );
    
    setProgress(value);
  }

  const getUserId = () => {
    // const token = cookies["test-cookies"];
    // if (!token) return;
    // const decoded = jwtDecode(token) as ICookieInfo;
    const userId = localStorage.getItem("userId");
    setCurrentUserId(userId);
  };

  const handleSaveStartDate = async (date: string) => {
    const taskId = searchParams.get("task");
    if (!taskId) return;
    const response = await fetch(
      `http://localhost:5000/api/task/${taskId}/start`,
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify(date)
      }
    );
  }

  const handleSaveFinishDate = async (date: string) => {
    const taskId = searchParams.get("task");
    if (!taskId) return;
    const response = await fetch(
      `http://localhost:5000/api/task/${taskId}/finish`,
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify(date)
      }
    );
  }

  const handleSaveChanges = async () => {
    const taskId = searchParams.get("task");
    if (!taskId) return;
    if(currentTask?.name !== name) saveName(taskId);
    if(currentTask?.description !== desc) saveDescription(taskId);
    // if (response1.ok && response2.ok) 
    setIsModified(false);
  };

  const handleChangeResponsibleUser = async (value: string) => {
    const taskId = searchParams.get("task");
    if (!taskId) return;
    if(value === "Все") {
      const response = await fetch(
        `http://localhost:5000/api/task/${taskId}/responsible?userId=${""}`,
        {
          method: "PUT",
          headers: { "Content-Type": "application/json" },
          credentials: "include",
        }
      );
      setResponsibleUser({} as IUser)
    }
    else{
      const newResponsibleUser = users.find(us => us.id === value);
      const newUsersArr = users.filter(user => user.id !== newResponsibleUser!.id);
      setUsers([...newUsersArr, responsibleUser])
      setResponsibleUser(newResponsibleUser!);
      const response = await fetch(
        `http://localhost:5000/api/task/${taskId}/responsible?userId=${value}`,
        {
          method: "PUT",
          headers: { "Content-Type": "application/json" },
          credentials: "include",
        }
      );
    }
  }

  const saveName = async (taskId: string) => {
    const response = await fetch(
      `http://localhost:5000/api/task/${taskId}/name?name=${name}`,
      {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      }
    );
    return response;
  }

  const saveDescription = async (taskId: string) => {
    const response = await fetch(
      `http://localhost:5000/api/task/${taskId}/description?description=${desc}`,
      {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      }
    );
    return response;
  }

  const getTaskInfo = async () => {
    const taskId = searchParams.get("task");
    if (!taskId) return;
    const response = await fetch(`http://localhost:5000/api/task/${taskId}`, {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    });
    const data = await response.json();
    console.log(data);
    getStagesInfo(data.task);
    setTask(data.task);
    setTaskComments(data.comments);
    const usersArr = await getUsers(params.id) as IUser[]
    const usersWithoutResponsible = usersArr.filter(us => us.id !== data.task.responsibleUserId);
    setUsers(usersWithoutResponsible);
    const user = usersArr.find(us => us.id === data.task.responsibleUserId);
    if(user) setResponsibleUser(user!);
    else setResponsibleUser({username: "Все"} as IUser);
  };

  const handleChangeStage = async (stageName: string | undefined) => {
    const taskId = searchParams.get("task");
    if (!taskId || stageName === undefined) return;
    const stage = statusList?.find((st) => st.name === stageName);
    const response = await fetch(
      `http://localhost:5000/api/stage/${stage?.id}?taskId=${taskId}`,
      {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      }
    );
    setStatus(stage);
  };

  const getStagesInfo = async (task: ITask) => {
    const response = await fetch(
      `http://localhost:5000/api/stage?projectId=${task.projectId}`,
      {
        method: "GET",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      }
    );
    const data = await response.json();
    setStatusList(data);
  };

  const getUserInfo = async (id: string) => {
    // const response = await fetch(`http://localhost:5000/api/user/${id}`, {
    //   method: "GET",
    //   headers: { "Content-Type": "application/json" },
    //   credentials: "include",
    // });
    // const data = await response.json();
    // setResponsibleUser(data);
  };

  const handleDelete = async () => {
    if (!currentTask) return;
    const response = await fetch(
      `http://localhost:5000/api/task/${currentTask.id}`,
      {
        method: "DELETE",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      }
    );
    console.log(response);
    setFetch(true);
    onClose();
  };

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
        //const str = {content: message, user: user} as ITaskComment;
        setTaskComments([...taskComments, comment]);
        // console.log(taskComments)
        // setChat((prevState) => ({
        //   ...prevState,
        //   messages: [...messages, str],
        // }));
      });
    }
  } catch (exception) {
    console.log(exception);
  }

  try {
    if (conn) {
      conn.on(`HelloMsg`, (message) => {
        console.log(message);
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
    className={"bg-white dark:bg-[#111]"}
    style={{width: "140vh", height: "80vh", display: "grid", gridTemplateColumns: "3fr 1fr", padding: "0.4rem"}}>
      <div
        className="border-r dark:border-gray-100 h-full flex flex-col"
        style={rights.canModifyTask ? {} : {pointerEvents: "none"}}
      >
        <div
          style={{
            display: "flex",
            flexDirection: "row",
            justifyContent: "space-between",
            alignItems: "center",
            margin: "0.4rem",
          }}
        >
          {/* <Label
            style={{
              color: "black",
              fontSize: "2rem",
              flexGrow: "5",
              textAlign: "center",
            }}
          >
            {currentTask?.name}
          </Label> */}
          {/* <X
            onClick={() => {
              onClose();
              setFetch(true);
            }}
            className={styles.closeBtn}
          /> */}
        </div>
        <div
          style={{
            display: "flex",
            flexDirection: "column",
            justifyContent: "space-evenly",
          }}
        >
          <div className="flex flex-row items-center w-[80%] mx-[3rem] my-[1rem]">
            <Label
              style={{
                color: "inherit",
                fontSize: "2rem",
                flexGrow: "5",
                // textAlign: "flex-start",
              }}
            >
              Название
            </Label>
            <Input
              className="text-red text-2xl grow-[5] text-center rounded-none border-0 focus-visible:outline-none focus-visible:ring-0 focus-visible:ring-offset-0"
              onChange={(e) => setName(e.target.value)}
              value={name}
              placeholder={name}
            />
          </div>
          <div style={{ width: "80%", margin: "1rem 3rem" }}>
            <Label
              style={{
                color: "inherit",
                fontSize: "2rem",
                flexGrow: "5",
                // textAlign: "flex-start",
              }}
            >
              Описание
            </Label>
            <Editor desc={desc} setDesc={setDesc} styles="dark:border-black/40"/>
          </div>
          <div style={{ display: "flex", flexDirection: "row" }}>
            <div
              style={{
                width: "40%",
                padding: "0.3rem 0",
                margin: "1rem 3rem",
                display: "flex",
                flexDirection: "row",
                alignItems: "center",
                justifyContent: "space-between",
              }}
            >
            <div className="flex flex-col w-full">
              <Label
                id="responsibleinput"
                className="text-black mr-[1rem] mb-[0.2rem]"
              >
                Исполнитель
              </Label>
              <Select
                // className={styles.select}
                value={ responsibleUser ? responsibleUser.id : "Все"}
                onValueChange={(value) => handleChangeResponsibleUser(value)}
              >
                <SelectTrigger className="dark:bg-[#111] dark:border-black/20">
                  <SelectValue placeholder="Выберите"></SelectValue>
                </SelectTrigger>
                <SelectContent>
                  {/* <SelectItem
                    key={responsibleUser.id}
                    value={responsibleUser.username}
                  >
                    <div
                      style={{
                        display: "flex",
                        flexDirection: "row",
                        alignItems: "center",
                        gap: 8,
                      }}
                    >
                      {responsibleUser.username !== "Все" ? (
                        <Avatar
                          style={{
                            backgroundColor: "#4198FF",
                            color: "white",
                            width: "2.5vh",
                            height: "2.5vh",
                            fontSize: "0.6rem",
                            margin: "0.1rem",
                            marginLeft: "0.4rem",
                            display: "flex",
                            justifyContent: "center",
                            alignItems: "center",
                          }}
                          // src="/static/images/avatar/1.jpg"
                        >
                          {responsibleUser?.firstName?.slice(0, 1)}
                          {responsibleUser?.lastName?.slice(0, 1)}
                        </Avatar>
                      ) : (
                        <div></div>
                      )}
                      <div>
                        {responsibleUser.username !== "Все"
                          ? responsibleUser.lastName +
                            " " +
                            responsibleUser.firstName
                          : "Все"}
                      </div>
                    </div>
                  </SelectItem> */}
                  <SelectItem
                    key={"all"}
                    value={"Все"}
                  >
                    <div
                      style={{
                        display: "flex",
                        flexDirection: "row",
                        alignItems: "center",
                        gap: 8,
                      }}
                    >
                      <div>
                         Все
                      </div>
                    </div>
                  </SelectItem>
                  { responsibleUser.firstName !== undefined ? 
                    <SelectItem
                    key={responsibleUser.id}
                    value={responsibleUser.id}
                  >
                    <div
                      style={{
                        display: "flex",
                        flexDirection: "row",
                        alignItems: "center",
                        gap: 8,
                      }}
                    >
                        <Avatar
                          style={{
                            backgroundColor: "#4198FF",
                            color: "white",
                            width: "2.5vh",
                            height: "2.5vh",
                            fontSize: "0.6rem",
                            margin: "0.1rem",
                            marginLeft: "0.4rem",
                            display: "flex",
                            justifyContent: "center",
                            alignItems: "center",
                          }}
                          // src="/static/images/avatar/1.jpg"
                        >
                          {responsibleUser?.firstName?.slice(0, 1)}
                          {responsibleUser?.lastName?.slice(0, 1)}
                        </Avatar>
                      <div>
                          {responsibleUser.lastName + " " + responsibleUser.firstName}
                      </div>
                    </div>
                  </SelectItem> : <></>}
                  {users.map(user => (
                    <SelectItem
                    key={user.id}
                    value={user.id}
                  >
                    <div
                      style={{
                        display: "flex",
                        flexDirection: "row",
                        alignItems: "center",
                        gap: 8,
                      }}
                    >
                      {user.username !== "Все" ? (
                        <Avatar
                          style={{
                            backgroundColor: "#4198FF",
                            color: "white",
                            width: "2.5vh",
                            height: "2.5vh",
                            fontSize: "0.6rem",
                            margin: "0.1rem",
                            marginLeft: "0.4rem",
                            display: "flex",
                            justifyContent: "center",
                            alignItems: "center",
                          }}
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
                          ? user.lastName +
                            " " +
                            user.firstName
                          : "Все"}
                      </div>
                    </div>
                  </SelectItem>
                  ))}
                </SelectContent>
              </Select>
              </div>
            </div>
            <div
              style={{
                width: "40%",
                padding: "0.3rem 0",
                margin: "1rem 3rem",
                display: "flex",
                flexDirection: "row",
                // borderBottom: "1px solid rgba(1, 1, 1, 0.1)",
                alignItems: "center",
                justifyContent: "space-between",
              }}
            >
            <div className="flex flex-col w-full">
              <Label
                id="statusinput"
                className="text-black mr-[1rem] mb-[0.2rem]"
              >
                Статус
              </Label>
              <Select
                // className={styles.select}
                // placeholder="Выберите"
                value={status?.name || ""}
                onValueChange={(value) => handleChangeStage(value)}
              >
                <SelectTrigger className="SelectTrigger dark:bg-[#111] dark:border-black/20" aria-label="Food">
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
          <Label style={{ marginBottom: "0.2rem" }}>Прогресс</Label>
          <Select
                // className={styles.select}
                value={progress}
                onValueChange={(value) => {
                  changeProgress(value);
                }}
              >
                {/* <SelectValue placeholder="Выберите" ></SelectValue> */}
                <SelectTrigger className="SelectTrigger dark:bg-[#111] dark:border-black/20" aria-label="Food">
                  <SelectValue placeholder="Прогресс" />
                </SelectTrigger>
                <SelectContent>
                    <SelectItem
                        value={"0"}
                      >
                        0
                    </SelectItem>
                    <SelectItem
                        value={"25"}
                      >
                        25
                    </SelectItem>
                    <SelectItem
                        value={"50"}
                      >
                        50
                    </SelectItem>
                    <SelectItem
                        value={"75"}
                      >
                        75
                    </SelectItem>
                    <SelectItem
                        value={"100"}
                      >
                        100
                    </SelectItem>
                </SelectContent>
              </Select>
          </div>
          <div style={{ display: "flex", flexDirection: "row" }}>
            <div
              style={{
                display: "flex",
                flexDirection: "column",
                margin: "1rem 3rem",
                padding: "0.6rem 0",
                width: "40%",
              }}
            >
              <Label style={{ marginBottom: "0.2rem" }}>Начало</Label>
              <Popover>
                <PopoverTrigger asChild className="dark:bg-[#111] dark:border-black/20 dark:hover:bg-black/20">
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
                        setStartDate(dayjs(value).format("YYYY-MM-DD"))
                        handleSaveStartDate(dayjs(value).format("YYYY-MM-DD"));
                      }
                    }
                  />
                </PopoverContent>
              </Popover>
            </div>
            <div
              style={{
                display: "flex",
                flexDirection: "column",
                margin: "1rem 3rem",
                padding: "0.6rem 0",
                width: "40%",
              }}
            >
              <Label style={{ marginBottom: "0.2rem" }}>Конец</Label>
              <Popover>
                <PopoverTrigger asChild className="dark:bg-[#111] dark:border-black/20  dark:hover:bg-black/20">
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
                        setFinishDate(dayjs(value).format("YYYY-MM-DD"))
                        handleSaveFinishDate(dayjs(value).format("YYYY-MM-DD"));
                      }
                    }
                  />
                </PopoverContent>
              </Popover>
            </div>
          </div>
          <div
            style={{
              display: "flex",
              justifyContent: "space-between",
              marginRight: "4rem",
              marginLeft: "3rem",
            }}
          >
            <Button
              className={isModified ? "border border-green-500 bg-white text-green-500 hover:bg-green-500 hover:text-white ease-in-out duration-300 dark:bg-green-500 dark:text-white dark:hover:bg-green-600" : "invisible"}
              onClick={() => handleSaveChanges()}
            >
              Применить
            </Button>
            <Button
              variant={"destructive"}
              className={rights.canDeleteTask ? "" : "invisible"}
              onClick={() => setIsModalOpen(true)}
            >
              Удалить
            </Button>
          </div>
        </div>
      </div>
      <Dialog
        open={isModalOpen}
        onOpenChange={setIsModalOpen}
      >
        <DialogContent className="sm:max-w-[425px]">
          <div
            style={{
              display: "flex",
              flexDirection: "column",
              justifyContent: "center",
              backgroundColor: "white",
              width: "100%",
              gap: 8,
              borderRadius: "8px",
              padding: "10px",
            }}
          >
            <Label style={{ color: "black", alignSelf: "center" }}>
              Вы действительно хотите удалить задачу?
            </Label>
            <div style={{ display: "flex", alignSelf: "center" }}>
              <Button
                onClick={() => handleDelete()}
                style={{
                  color: "white",
                  backgroundColor: "green",
                  marginRight: "0.4rem",
                }}
              >
                Да
              </Button>
              <Button
                onClick={() => setIsModalOpen(false)}
                style={{ color: "white", backgroundColor: "red" }}
              >
                Нет
              </Button>
            </div>
          </div>
        </DialogContent>
      </Dialog>
      <div className={styles.chat}>
        <div className={styles.container}>
          <div
            style={{
              display: "flex",
              flexDirection: "row",
              padding: "0.6rem 0.6rem",
              justifyContent: "space-between",
            }}
          >
            <Label
              style={{
                fontSize: "1.2rem",
                marginLeft: "2rem",
                //color: "rgb(114, 115, 118)",
              }}
            >{`Комментарии`}</Label>
            <X onClick={ () => {
              onClose();
              setFetch(true)
              }
              } className={styles.closeBtn} />
          </div>
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
            {taskComments.map((message, index) =>
              currentUserId && message.userId === currentUserId ? (
                <div key={index} className="flex flex-col">
                <div className="self-end">
                    <Label
                      style={{
                        textAlign: "center",
                        fontSize: ".7rem",
                        color: "#87888C",
                      }}
                    >
                      {message ? dayjs(message.time).format("DD.MM.YY HH:MM") : ""}
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
                          className="text-white dark:text-white"
                          style={{
                            textTransform: "capitalize",
                            fontWeight: "500",
                            fontSize: ".9rem",
                          }}
                        >
                          {message.content}
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
                        fontSize: ".7rem",
                        color: "#87888C",
                      }}
                    >
                      {message ? dayjs(message.time).format("DD.MM.YY HH:MM") : ""}
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
                            textTransform: "capitalize",
                            fontWeight: "500",
                            fontSize: ".9rem",
                            color: "black",
                          }}
                        >
                          {message.content}
                        </Label>
                        <Label
                          style={{
                            textTransform: "capitalize",
                            fontWeight: "500",
                            fontSize: ".6rem",
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
          <div style={{ display: "flex", justifyContent: "center" }}>
            <div
              style={{
                // backgroundColor: "#ECF0F3",
                padding: ".4rem .8rem",
                borderRadius: ".6rem",
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
                margin: ".5rem 0 1vh 0",
                width: "80%",
                gap: 8,
              }}
            >
              <Input
                className="flex-1 dark:text-white dark:bg-[#111] dark:border-black/20"
                //style={{ flex: 1, fontSize: ".9rem" }}
                placeholder="Написать сообщение..."
                // inputProps={{ "aria-label": "Написать сообщение..." }}
                value={message}
                onChange={(e) => setMessage(e.target.value)}
              />
              <Send
                style={{
                  fontSize: ".9rem",
                  color: "rgba(102, 153, 255, 0.6)",
                  // "&:hover": { color: "rgba(102, 153, 255, 0.3)" },
                }}
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
