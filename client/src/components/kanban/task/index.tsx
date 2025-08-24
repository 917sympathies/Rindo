"use client";
import {
  useState,
  useEffect,
  Dispatch,
  SetStateAction,
  useCallback,
} from "react";
import { ITask, IUser, UserRights } from "@/types";
import { Dialog, DialogContent } from "@/components/ui/dialog";
import { Avatar } from "@/components/ui/avatar";
import { MessageCircle } from "lucide-react";
import TaskModal from "@/components/taskModal";
import { usePathname, useSearchParams } from "next/navigation";
import { useRouter } from "next/navigation";
import { EllipsisVertical } from "lucide-react";
import { Progress } from "@/components/ui/progress";
import { GetTasksCommentsAmount, GetUserInfo } from "@/requests";

interface ITaskProps {
  task: ITask;
  setFetch: Dispatch<SetStateAction<boolean>>;
  rights: UserRights | undefined;
}

function Task({ task, setFetch, rights }: ITaskProps) {
  const searchParams = useSearchParams();
  const pathname = usePathname();
  const router = useRouter();
  const [responsibleUser, setResponsibleUser] = useState<IUser | null>(null);
  const [commentsAmount, setCommentsAmount] = useState<number | null>(null);
  const [commentLabel, setCommentLabel] = useState<string>("комментариев");

  useEffect(() => {
    if (task.responsibleUserId !== null) getUserInfo(task.responsibleUserId);
    getCommentsCount();
  }, []);

  const getUserInfo = async (id: string) => {
    const response = await GetUserInfo(id);
    const data = await response.json();
    setResponsibleUser(data);
  };

  const getCommentsCount = async () => {
    const response = await GetTasksCommentsAmount(task.id);
    const data = await response.json();
    setCommentsAmount(data);
    changeLabel(data);
  };

  const changeLabel = (data: number) => {
    if (data === 0 || (data > 4 && data < 21)) setCommentLabel("комментариев");
    else if (data > 1 && data < 5) setCommentLabel("комментария");
    else if (data === 1) setCommentLabel("комментарий");
    else {
      const val = data % 10;
      if (val === 0 || (val > 4 && val < 21)) setCommentLabel("комментариев");
      else if (val > 1 && val < 5) setCommentLabel("комментария");
      else if (val === 1) setCommentLabel("комментарий");
    }
  };

  const handleOpenModal = useCallback(
    (open: boolean) => {
      const params = new URLSearchParams(searchParams.toString());
      if (open) params.set("task", task.id);
      else params.delete("task");
      return params.toString();
    },
    [searchParams]
  );

  return (
    <>
      <div className="bg-white rounded-lg mt-[10px] w-full self-center shadow">
        <div className="flex justify-between items-center text-black text-[0.95rem] font-semibold m-0 pt-[0.8rem] px-[1rem] pb-0 text-clip">
          <div>{task.name}</div>
          <div
            className="h-fit w-fit flex justify-center p-[0.2rem] rounded-[6px] hover:cursor-pointer hover:bg-[rgba(1,1,1,0.03)] ease-in-out duration-200"
            onClick={() => router.push(pathname + "?" + handleOpenModal(true))}
          >
            <EllipsisVertical size={16} color="rgb(102,102,102)" />
          </div>
        </div>
        <div className="text-[0.7rem] my-[0.2rem] mx-[0.8rem]">
          <textarea
            value={task.description}
            disabled
            cols={30}
            maxLength={100}
            className="resize-none border-0 overflow-hidden h-auto bg-inherit"
          ></textarea>
        </div>
        <div className="flex flex-row justify-between content-center pb-[0.4rem] my-0 mx-[0.4rem]">
          <div>
            {responsibleUser == null ? (
              <>
                {/* <p
                  style={{
                    margin: "0 0.4rem",
                    fontSize: "0.8rem",
                    padding: "0 0.6rem",
                    backgroundColor: "yellow",
                    borderRadius: "16px",
                    
                  }}
                >
                  Для всех
                </p> */}
              </>
            ) : (
              <div className="flex flex-row items-center my-[0.2rem] mx-0">
                <Avatar className="bg-[#4198FF] text-white w-[2.5vh] h-[2.5vh] text-[0.6rem] m-[0.1rem] ml-[0.4rem] flex justify-center items-center"  
                  // src="/static/images/avatar/1.jpg"
                >
                  {responsibleUser?.firstName?.slice(0, 1)}
                  {responsibleUser?.lastName?.slice(0, 1)}
                </Avatar>
              </div>
            )}
          </div>
          <div className="w-full flex justify-center items-center">
            <Progress
              className="h-1 w-3/5 dark:bg-gray-300 mb-1"
              value={task.progress}
            />
          </div>
          <div className="flex flex-row justify-end items-center">
            <MessageCircle className="mr-[0.4rem]" size={16} />
            <div className="mr-[0.4rem] text-[0.8rem] text-hidden overflow-hidden whitespace-nowrap">
              {commentsAmount
                ? commentsAmount + " " + commentLabel
                : "0 комментариев"}
            </div>
          </div>
        </div>
      </div>
      <Dialog open={searchParams.get("task") === task.id}>
        <DialogContent className="w-[140vh]"
          onOpenAutoFocus={(e) => e.preventDefault()}
        >
          <TaskModal
            onClose={() => router.push(pathname + "?" + handleOpenModal(false))}
            setFetch={setFetch}
            rights={rights}
          />
        </DialogContent>
      </Dialog>
    </>
  );
}

export default Task;
