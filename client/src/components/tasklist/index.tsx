"use client";
import { useCallback } from "react";
import { ITask, IUserInfo, IUserRights } from "@/types";
import {
  useParams,
  usePathname,
  useRouter,
  useSearchParams,
} from "next/navigation";
import { useState, useEffect } from "react";
import { Avatar } from "../ui/avatar";
import { Dialog, DialogContent } from "../ui/dialog";
import TaskModal from "../taskModal";
import { EllipsisVertical } from "lucide-react";
import { Progress } from "../ui/progress";
import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import dayjs from "dayjs";

interface TaskListProps {}

interface ITaskDto{
  task: ITask,
  user: IUserInfo
}

export default function TaskList({}: TaskListProps) {
  const { id } = useParams<{ id: string }>();
  const [toFetch, setFetch] = useState<boolean>(false);
  const [tasks, setTasks] = useState<ITaskDto[]>([] as ITaskDto[]);
  const [rights, setRights] = useState<IUserRights>({} as IUserRights);
  const pathname = usePathname();
  const router = useRouter();
  const searchParams = useSearchParams();

  useEffect(() => {
    getTasks(id);
    getRights();
  }, [id]);

  useEffect(() => {
    if (toFetch) {
      getTasks(id);
      setFetch(false);
    }
  }, [toFetch]);

  const getRights = async () => {
    const userId = localStorage.getItem("userId");
    const response = await fetch(
      `http://localhost:5000/api/role/${id}/${userId}`,
      {
        method: "GET",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      }
    );
    const data = await response.json();
    setRights(data);
  };

  const getTasks = async (id: string) => {
    const response = await fetch(
      `http://localhost:5000/api/task/?projectId=${id}`,
      {
        method: "GET",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      }
    );
    const data = await response.json();
    setTasks(data);
  };

  const handleOpenModal = useCallback(
    (taskId?: string) => {
      const params = new URLSearchParams(searchParams.toString());
      if (taskId) params.set("task", taskId);
      else params.delete("task");
      return params.toString();
    },
    [searchParams]
  );

  return (
    <>
      <div className="px-20">
        <Table>
          <TableCaption>Список задач проекта</TableCaption>
          <TableHeader>
            <TableRow className="w-full">
              <TableHead className="w-[15%]">Имя</TableHead>
              <TableHead className="w-[15%]">Дата начала</TableHead>
              <TableHead className="w-[15%]">Дата конца</TableHead>
              <TableHead className="w-[25%]">Ответственный</TableHead>
              <TableHead className="w-[15%]">Прогресс</TableHead>
              <TableHead className=""></TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {tasks?.map((ent) => (
              <TableRow key={ent.task.id}>
                <TableCell className="font-medium overflow-hidden">
                  {ent.task.name}
                </TableCell>
                <TableCell className="font-medium">
                  {dayjs(ent.task.startDate).format("DD.MM.YYYY")}
                </TableCell>
                <TableCell className="font-medium">
                  {dayjs(ent.task.finishDate).format("DD.MM.YYYY")}
                </TableCell>
                <TableCell className="flex flex-row items-center gap-2">
                  {ent.user ?
                  <div className="flex flex-row items-center gap-2">
                    <Avatar className="bg-[#4198FF] text-white w-[2.5vh] h-[2.5vh] text-[0.6rem] flex justify-center items-center">
                      {ent.user?.firstName?.slice(0, 1)}
                      {ent.user?.lastName?.slice(0, 1)}
                    </Avatar>
                    <div>{ent.user?.firstName + " " + ent.user?.lastName}</div>
                  </div> :
                  <div>
                    Нет ответственного
                  </div> }
                </TableCell>
                <TableCell>
                  <Progress
                    className="w-full h-[0.6rem]"
                    value={ent.task.progress}
                  />
                </TableCell>
                <TableCell className="w-full">
                  <EllipsisVertical
                    className="p-1 hover:bg-gray-100 rounded-full ease-in-out duration-300 text-gray-500"
                    size={24}
                    onClick={() =>
                      router.push(pathname + "?" + handleOpenModal(ent.task.id))
                    }
                  />
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>
      <div>
        <Dialog open={searchParams.has("task")}>
          <DialogContent>
            <div className="flex justify-center self-center content-center">
              <TaskModal
                onClose={() => router.push(pathname + "?" + handleOpenModal())}
                setFetch={setFetch}
                rights={rights}
              ></TaskModal>
            </div>
          </DialogContent>
        </Dialog>
      </div>
    </>
  );
}
