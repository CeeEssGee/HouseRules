import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { createChore, getChores } from "../../managers/choreManager";
import { Button, Form, FormGroup, Input, Label } from "reactstrap";

export const CreateChore = () => {
    const [name, setName] = useState("");
    const [difficulty, setDifficulty] = useState(null);
    const [choreFrequencyDays, setFrequencyDays] = useState(null);
    const [chores, setChores] = useState([]);

    const navigate = useNavigate();

    const handleSubmit = (e) => {
        e.preventDefault();
        const newChore = {
            name,
            difficulty,
            choreFrequencyDays
        };

        createChore(newChore).then(() => {
            navigate("/chores");
        });
    };

    useEffect(() => {
        getChores().then(setChores);
    }, []);


    return (
        <>
            <h2>Create Chore</h2>
            <Form>
                <FormGroup>
                    <Label>Name</Label>
                    <Input
                        type="text"
                        placeholder="Name of Chore..."
                        onChange={(e) => {
                            setName(e.target.value);
                        }}
                    />
                </FormGroup>
                <FormGroup>
                    <Label>Difficulty</Label>
                    <Input
                        type="select"
                        onChange={(e) => {
                            setDifficulty(parseInt(e.target.value));
                        }}>
                        <option value={0}>Select Difficulty Level</option>
                        <option value={1}>1</option>
                        <option value={2}>2</option>
                        <option value={3}>3</option>
                        <option value={4}>4</option>
                        <option value={5}>5</option>

                    </Input>
                </FormGroup>
                <FormGroup>
                    <Label htmlFor="frequency">Chore Frequency</Label>
                    <Input
                        type="number"
                        list="frequency-numbers"
                        name="frequency"
                        onChange={(e) => {
                            setFrequencyDays(parseInt(e.target.value));
                        }}
                    />
                    <datalist id="frequency-numbers">
                        <option value={1}></option>
                        <option value={3}></option>
                        <option value={7}></option>
                        <option value={10}></option>
                        <option value={14}></option>
                    </datalist>
                </FormGroup>
                <Button onClick={handleSubmit} color="primary">
                    Submit
                </Button>
            </Form>
        </>
    )
}