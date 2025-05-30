"use client";
import { useEffect, useState } from "react";
import Link from "next/link";
import { useRouter, useParams, redirect } from "next/navigation";
import { Button } from "../ui/button";
import { Sheet, SheetContent } from "../ui/sheet";
import { SquareActivity, Plus, ClipboardList } from "lucide-react";
import AddProjectModal from "../addProjectModal";
import { useCookies } from "react-cookie";
import { jwtDecode } from "jwt-decode";
import { IUser } from "@/types";
import { ICookieInfo } from "@/types";
import { ModeToggle } from "../modeToggle";
import { Avatar } from "../ui/avatar";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import {
  HubConnectionBuilder,
  HubConnection,
  LogLevel,
  HubConnectionState,
} from "@microsoft/signalr";
import { GetProjectsByUserId } from "@/requests";

interface ISidebarProps {}

interface IProjectInfo {
  id: string;
  name: string;
}

const Sidebar = ({}: ISidebarProps) => {
  const router = useRouter();
  const [cookies, setCookie, removeCookie] = useCookies(["_rindo"]);
  const { id } = useParams();
  const [user, setUser] = useState<IUser>();
  const [projects, setProjects] = useState<IProjectInfo[] | []>(
    [] as IProjectInfo[]
  );
  const [conn, setConnection] = useState<HubConnection | null>(null);
  const [isOpen, setIsOpen] = useState(false);
  const [toFetch, setFetch] = useState(false);

  useEffect(() => {
    const temp = localStorage.getItem("user");
    if (!temp) return;
    const u = JSON.parse(temp);
    setUser(u);
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
      conn.on(`ReceiveDeleteProject`, (projectId) => {
        setProjects(projects?.filter((p) => p.id !== projectId));
      });
      conn.on(`ReceiveAcceptInvite${user?.id}`, (projectId, name) => {
        const project: IProjectInfo = { id: projectId, name: name };
        setProjects([...projects, project]);
      });
      conn.on(`ReceiveChangeProjectName`, (projectId, name) => {
        setProjects(
          projects?.map((project) => {
            if (project.id !== projectId) return project;
            else {
              return {
                ...project,
                name: name,
              };
            }
          })
        );
      });
    }
  } catch (exception) {
    console.log(exception);
  }

  useEffect(() => {
    const fetchProjectsInfo = async () => {
      const token = cookies["_rindo"];
      if (token === undefined) {
        router.push("/login");
        redirect("/login");
      }
      const decoded: ICookieInfo = jwtDecode(token);
      const response = await GetProjectsByUserId(decoded.userId);
      if (response.status === 401 || response.status === 404) {
        removeCookie("_rindo", { path: "/" });
        router.push("/login");
      }
      const data = await response.json();
      setProjects(data);
    };

    if (!user || toFetch) {
      fetchProjectsInfo();
      setFetch(false);
    }
  }, [toFetch]);

  const logout = () => {
    removeCookie("_rindo", { path: "/" });
    localStorage.removeItem("user");
    localStorage.removeItem("userId");
    localStorage.removeItem("token");
    router.push("/login");
  };

  return (
    <div className="pt-[10px] pb-[10px] flex flex-col items-center justify-between border-r border-[rgba(1,1,1,0.1)] w-full dark:border-gray-400">
      <ul className="list-none w-[90%] max-h-[80vh]">
        {/* <div className="flex flex-row items-center justify-center">
          <svg
            xmlns="http://www.w3.org/2000/svg"
            width="36"
            height="36"
            viewBox="0 0 100 100"
            fill="none"
          >
            <rect width="36" height="36" fill="white" />
            <path
              d="M42.5 16H59L30 75H12L42.5 16Z"
              fill="#88FFD4"
              fill-opacity="0.7"
            />
            <path
              d="M54.6383 46.514C44.2979 45.3575 42 46.514 42 46.514L64.2128 92H78C78 92 64.9787 47.6704 54.6383 46.514Z"
              fill="#88FFD4"
              fill-opacity="0.5"
            />
            <path
              d="M56.5 16H93.5C93.5 16 101 18 93.5 27C86 36 51 27 51 27L56.5 16Z"
              fill="#88FFD4"
              fill-opacity="0.3"
            />
            <path
              d="M70 24C68 17 60 24 86 24C112 24 57.5 48 57.5 48C57.5 48 38 46 42.5 46.5C47 47 72 31 70 24Z"
              fill="#88FFD4"
              fill-opacity="0.4"
            />
          </svg>
        </div> */}
        <li className="items-center justify-between grid grid-cols-[9fr_1fr]">
          <DropdownMenu>
            <DropdownMenuTrigger asChild>
              <div className="m-0 text-[#727376] font-inherit flex flex-row items-center gap-2 p-1 rounded-lg hover:bg-gray-50 ease-in-out duration-300 dark:hover:bg-black/20 cursor-pointer">
                <Avatar className="rounded-lg bg-[#4198FF] text-white w-[2.5vh] h-[2.5vh] text-[0.6rem] m-[0.1rem] flex justify-center items-center">
                  {user?.username?.slice(0, 1)}
                </Avatar>
                <h1 className="m-0 p-0 text-[1.1rem] flex-grow text-start">{user?.username}</h1>
              </div>
            </DropdownMenuTrigger>
            <DropdownMenuContent className="w-56 dark:bg-[#111] dark:border-black/20">
              <DropdownMenuLabel>{user?.username}</DropdownMenuLabel>
              <DropdownMenuSeparator className="dark:bg-black/20" />
              <DropdownMenuGroup>
                <DropdownMenuItem>
                  <Link href="/profile" className="w-full" passHref={true}>
                    Profile
                  </Link>
                </DropdownMenuItem>
              </DropdownMenuGroup>
              <DropdownMenuSeparator className="dark:bg-black/20" />
              <DropdownMenuItem>
                <Link
                  href="https://github.com/917sympathies/Rindo"
                  className="w-full"
                  passHref={true}
                  target="_blank"
                >
                  GitHub
                </Link>
              </DropdownMenuItem>
              <DropdownMenuItem>Support</DropdownMenuItem>
              <DropdownMenuSeparator className="dark:bg-black/20" />
              <DropdownMenuItem onClick={() => logout()}>
                Logout
              </DropdownMenuItem>
            </DropdownMenuContent>
          </DropdownMenu>
          <ModeToggle />
        </li>
        <li className="mt-6">
          <div className="flex items-center justify-between">
            <div className="text-[#727376] font-medium text-[1rem]">
              All Projects
            </div>
          </div>
        </li>
        {projects &&
          projects.map((project) => (
            <div
              className={
                project.id == id
                  ? "flex flex-row items-center py-[4px] pr-[10px] pl-[30px] text-[0.9rem] font-medium text-clip rounded-[6px] text-[rgb(114,115,118)] my-[10px] bg-black/10 text-black"
                  : "flex flex-row items-center py-[4px] pr-[10px] pl-[30px] text-[0.9rem] font-medium text-clip rounded-[6px] text-[rgb(114,115,118)] my-[10px]"
              }
              key={project.id}
            >
              <Link
                href={`/project/${project.id}/board`}
                className="no-underline w-full flex items-center dark:text-white"
              >
                <SquareActivity className="mr-3" size={20} />
                <div className="text-[1rem]">{project.name}</div>
              </Link>
            </div>
          ))}
        <Button
          className="w-full rounded-md text-white bg-[#3A86FF] hover:bg-blue-800 ease-in-out duration-300 justify-start"
          onClick={() => setIsOpen(true)}
        >
          <div className="flex flex-row items-center justify-between w-full">
            <span className="font-normal text-[1rem]">New project</span>
            <Plus size={18} />
          </div>
        </Button>
        <div className="flex flex-row items-center justify-start p-4 text-[1rem] text-[#727376] w-full dark:text-white">
          <Link href="/tasks" className="flex flex-row items-center w-full justify-between">
            <span>My tasks</span>
            <ClipboardList size={18}/>
          </Link>
        </div>
      </ul>
      <Sheet key="right" open={isOpen}>
        <SheetContent
          className="h-screen top-0 right-0 left-auto mt-0 w-[500px] rounded-none"
          side={"right"}
        >
          <AddProjectModal
            onClose={() => setIsOpen(false)}
            setFetch={setFetch}
          />
        </SheetContent>
      </Sheet>
    </div>
  );
};

export default Sidebar;
