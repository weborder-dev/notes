<div class="content">
    <div class="list">
        <div class="options-container">
            <button on:click={newNote}>Nueva nota</button>
        </div>
        <ul>
            {#each notesList as single}
                <li on:click={fetchRecord(single.id)}>{single.title}</li>
            {/each}
        </ul>
    </div>
    <div class="note">
        <input bind:this={titleRef} placeholder="Título" type="text" bind:value={noteTitle} />
        <textarea placeholder="Notas" bind:value={noteBody}></textarea>
        <button on:click={saveNote}>Guardar</button>
    </div>
</div>

<script lang="ts">
    import { onMount } from "svelte";

    const apiURL = "https://4f03-24-242-110-215.ngrok-free.app";
    let titleRef: HTMLInputElement;

    let notesList: any = [];
    let noteTitle: string = "";
    let noteBody: string = "";

    onMount(() => {
        fetchRecords();
    });

    const newNote = () => {
        noteTitle = "";
        noteBody = "";
        titleRef.focus();
    }

    const saveNote = () => {
        console.log("Guarda la nota");
        saveRecord();
    }

    const fetchRecord = async (id: string) => {
        console.log(id);
        const response = await fetch(apiURL + '/notes/' + id, {
            method: 'POST',
            headers: {
            'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error('Error en la solicitud');
        }

        console.log(response);

        const data = await response.json();
        console.log(data);
        noteTitle = data.title;
        noteBody = data.body;
    };

    const fetchRecords = async () => {
        const response = await fetch(apiURL + '/notes/all', {
            method: 'POST',
            headers: {
            'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error('Error en la solicitud');
        }

        const data = await response.json();
        notesList = data;
    };

    const saveRecord = async () => {
        const response = await fetch(apiURL + '/notes', {
            method: 'POST',
            headers: {
            'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                "title": noteTitle,
                "body": noteBody
            })
        });

        if (!response.ok) {
            throw new Error('Error en la solicitud');
        }

        const data = await response.json();
        fetchRecords();
    };
</script>

<style lang="scss">
    .content {
        display: flex;
        flex-direction: row;
        width: 100vw;
        min-height: 100vh;
        color: white;
    }
    
    .list {
        width: 30%;
        background: black;

        .options-container {
            text-align: center;
            padding: 1em;
        }

        ul {
            list-style-type: none;
            padding: 0;

            li {
                border-bottom: 1px solid blueviolet;
                font-size: 0.7em;
                cursor: pointer;
            }
        }
    }

    .note {
        padding: 1em;
        background: #222;
        flex-grow: 1;
        display: flex;
        flex-direction: column;

        input {
            width: 100%;
            background: #333;
            border: none;
            color: white;
        }

        textarea {
            width: 100%;
            flex-grow: 1;
            background: #333;
            border: none;
            color: white;
        }

        input, textarea {
            outline: none;
        }
    }

    button {
        background: blueviolet;
        color: white;
        border: none;
        padding: 0.3em 1em;
        border-radius: 1em;
    }
</style>