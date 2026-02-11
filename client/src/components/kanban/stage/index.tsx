import {Dispatch, SetStateAction, useEffect, useState} from 'react'
import {Stage, StageDto, StageType, UserRights} from "@/types";
import {Button} from "@/components/ui/button";
import {Dialog, DialogContent} from "@/components/ui/dialog";
import {Label} from "@/components/ui/label";
import {Draggable, Droppable} from "react-beautiful-dnd";
import {TurnOffDefaultPropsWarning} from "@/components/turnOffDefaultPropsWarning";
import {Plus, X} from 'lucide-react'
import {ScrollArea} from "@/components/ui/scroll-area";
import {HubConnection, HubConnectionBuilder, HubConnectionState} from "@microsoft/signalr";
import Task from '../task';

interface IStageProps {
  stage: StageDto;
  handleDeleteStage: (id: string) => void;
  onClick: () => void;
  setFetch: Dispatch<SetStateAction<boolean>>;
  rights: UserRights | undefined
}

export default function Stage({ stage, onClick, handleDeleteStage, setFetch, rights }: IStageProps) {
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
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

  try {
    if (conn) {
      conn.on(`ReceiveTaskAdd${stage.projectId}`, (needToFetch) => {
        setFetch(needToFetch);
      });
    }
  } catch (exception) {
    console.log(exception);
  }


  return (
    <div className="bg-[rgba(1,1,1,0.03)] min-w-[300px] max-w-[300px] flex flex-col justify-center items-center text-[rgb(102,102,102)] rounded-md dark:bg-black/50">
      <div className="flex items-center font-semibold justify-around w-full grow-1 py-[0.8rem] border-b-[0.25rem] border-white">
        <TurnOffDefaultPropsWarning />
        <p className="flex justify-center grow">
          {stage.customName}
        </p>
        {stage.type === StageType.Custom && rights && (rights & UserRights.CanDeleteStage) &&
          <X className="text-[1.2rem] p-[0.3rem] mr-[0.4rem] text-[rgb(114,115,118)] self-center hover: rounded-[12px] hover:bg-[rgba(1,1,1,0.03)] ease-in-out duration-200"
             size={24} onClick={() => setIsModalOpen(true)} />
        }
      </div>
      <div className="w-full max-h-[80vh] grow-[20] flex flex-col p-[0.6rem]">
        {stage.type === StageType.ToDo && rights && (rights & UserRights.CanAddTask) ?
          <Button variant={"default"} onClick={onClick}>
            <Plus size={16} />
          </Button>
          : <div></div>}
        <Droppable key={stage.customName} droppableId={stage.id}>
          {(provided) => (
            <ScrollArea className="h-[70vh] pt-[1rem]">
              <div
                ref={provided.innerRef}
                {...provided.droppableProps}
                className="min-h-[2rem] mr-[0.8rem] w-full flex flex-col gap-3"
              >
                {stage &&
                  stage.tasks?.map((task, index) => (
                    <Draggable key={task.taskId} draggableId={task.taskId} index={index}>
                      {(provided, snapshot) => (
                        <div
                            ref={provided.innerRef}
                            {...provided.dragHandleProps}
                            {...provided.draggableProps}
                        >
                          <Task key={task.taskId} task={task} setFetch={setFetch} rights={rights} />
                        </div>
                      )}
                    </Draggable>
                  ))}
                {provided.placeholder}
              </div>
            </ScrollArea>
          )}
        </Droppable>
      </div>
      <Dialog
        open={isModalOpen}
      >
        <DialogContent>
          <div
            className="gap-4"
            style={{
              display: "flex",
              flexDirection: "column",
              justifyContent: "center",
              alignItems: "center",
              backgroundColor: "white",
              width: "100%",
              minHeight: "6rem",
              borderRadius: "8px",
              padding: "10px",
            }}
          >
            <Label style={{ color: "black", alignSelf: "center" }}>
              Вы действительно хотите удалить стадию?
            </Label>
            <div style={{ display: "flex", alignSelf: "center" }}>
              <Button
                onClick={() => handleDeleteStage(stage.id)}
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
    </div>
  );
}
