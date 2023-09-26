import { useEffect, useState } from "react"
import { demoteUser, getUserProfilesWithRoles, promoteUser } from "../../managers/userProfileManager";
import { Button, Table } from "reactstrap";
import { Link, useNavigate } from "react-router-dom";


export const UserProfileList = () => {
    const [userProfiles, setUserProfiles] = useState([]);

    const navigate = useNavigate();

    useEffect(() => {
        getUserProfilesWithRoles().then(setUserProfiles);
    }, [])

    const promote = (id) => {
        promoteUser(id).then(() => {
            getUserProfilesWithRoles().then(setUserProfiles);
        });
    };

    const demote = (id) => {
        demoteUser(id).then(() => {
            getUserProfilesWithRoles().then(setUserProfiles);
        });
    };

    return (
        <>
            <h2>User Profiles</h2>
            <Table>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Address</th>
                        <th>Email</th>
                        <th>Username</th>
                        <th>Roles</th>
                        <th>Actions</th>
                        <th>Details</th>
                    </tr>
                </thead>
                <tbody>
                    {userProfiles.map((up) => (
                        <tr key={up.id}>
                            <th scope="row">{`${up.firstName} ${up.lastName}`}</th>
                            <td>{up.address}</td>
                            <td>{up.email}</td>
                            <td>{up.userName}</td>
                            <td>{up.roles}</td>
                            <td>
                                {up.roles.includes("Admin") ? (
                                    <Button
                                        color="danger"
                                        onClick={() => {
                                            demote(up.identityUserId);
                                        }}
                                    >
                                        Demote
                                    </Button>
                                ) : (
                                    <Button
                                        color="success"
                                        onClick={() => {
                                            promote(up.identityUserId);
                                        }}
                                    >
                                        Promote
                                    </Button>
                                )}
                            </td>
                            <td>
                                <Button
                                    color="info"
                                    onClick={() => {
                                        navigate(`${up.id}`);
                                    }}
                                >Details</Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </>
    )


}